using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MineStageStatusMachine : StageStatusMachine
{
    private const float ChangeInterval = 10f;

    private enum Status
    {
        Normal,
        Battle,
    }

    private MineStageStatusMachineData stageMachineData;

    private Mine mine;
    private int planetID;
    private MiningRobotController[] robotControllers = new MiningRobotController[2];

    private MineDefence mineDefence;

    private MiningBattleTable.Data battleData;
    private MiningBattleSpawnTable.Data battleSpawnData;

    private Status status;

    private float remainingTime;
    private float stageStartTime;

    private int centerHP;
    private float spawnIntervalReduceTime;
    private float spawnInterval;
    private float[] spawnTimers = new float[4];

    private float[] weight = new float[3] { 1f, 1f, 1f };

    public MineStageStatusMachine(StageManager stageManager) : base(stageManager)
    {
    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (MineStageStatusMachineData)stageMachineData;
    }

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        if (IsActive)
        {
            Start();
            stageManager.CameraManager.enabled = false;
            stageManager.CameraManager.SetCameraRotation(stageMachineData.cameraRotation);
            stageManager.CameraManager.SetCameraOffset(stageMachineData.cameraPosition);


            stageManager.StageUiManager.IngameUIManager.mineBattleButton.gameObject.SetActive(true);

            MiningRobotInventoryManager.onEquipRobot += OnEquipChanged;
        }
        else
        {
            stageManager.CameraManager.enabled = true;
            stageManager.CameraManager.SetCameraRotation();
            stageManager.CameraManager.SetCameraOffset();

            if (status == Status.Battle)
            {
                status = Status.Normal;
                stageManager.StageMonsterManager.StopMonster();
                stageManager.UnitPartyManager.UnitDespawn();
                stageManager.StageMonsterManager.ClearMonster();
            }

            stageManager.StageUiManager.IngameUIManager.mineBattleButton.gameObject.SetActive(false);
            stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(true);
            stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(0);
            stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(1);

            mine.Release();
            mine = null;
            for (int i = 0; i < robotControllers.Length; ++i)
            {
                robotControllers[i]?.Release();
                robotControllers[i] = null;
            }
        }
    }
    public override void Exit()
    {
        if (status == Status.Battle)
        {
            status = Status.Normal;
            stageManager.StageUiManager.curtain.SetFade(true);
            mineDefence.Release();
            stageManager.StageUiManager.ResourceRow.SetActive(true);
            stageManager.StageUiManager.IngameUIManager.mineBattleButton.gameObject.SetActive(true);
            stageManager.StageMonsterManager.StopMonster();
            stageManager.UnitPartyManager.UnitDespawn();
            stageManager.StageMonsterManager.ClearMonster();
            stageManager.StageUiManager.HPBarManager.ClearHPBar();

            stageManager.StageUiManager.IngameUIManager.miningBattleTimerGameObject.SetActive(false);
            stageManager.StageUiManager.IngameUIManager.miningBattleCenterHpBar.gameObject.SetActive(false);

            stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(true);
            stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(0);
            stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(1);
        }
        else
        {
            stageManager.SetStatus(IngameStatus.Planet);
        }
    }

    public override void Reset()
    {
        if (Variables.planetMiningID == planetID)
        {
            return;
        }
        stageManager.StageUiManager.curtain.SetFade(true);
        mine.Release();
        mine = null;
        for (int i = 0; i < robotControllers.Length; ++i)
        {
            robotControllers[i]?.Release();
            robotControllers[i] = null;
        }
        InitStage();
    }

    public override void Start()
    {
        InitStage();
        SoundManager.Instance.PlayBGM("MiningBGM");
    }

    public override void Update()
    {
        switch (status)
        {
            case Status.Battle:
                float currentTime = Time.time;
                UpdateTimer(currentTime);
                break;
        }
    }

    protected void InitStage()
    {
        planetID = Variables.planetMiningID;
        var planetData = DataTableManager.PlanetTable.GetData(planetID);
        var prefabAddress = DataTableManager.AddressTable.GetData(planetData.PrefabID);
        var mineGo = stageManager.ObjectPoolManager.Get(prefabAddress);
        mineGo.transform.parent = null;
        mine = mineGo.GetComponent<Mine>();
        var equipments = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet;

        if (!equipments.ContainsKey(planetID))
        {
            return;
        }

        for (int i = 0; i < equipments[planetID].Length; ++i)
        {
            if (equipments[planetID][i].isEmpty)
            {
                continue;
            }
            var robotData = DataTableManager.RobotTable.GetData(equipments[planetID][i].miningRobotId);
            var robotAddress = DataTableManager.AddressTable.GetData(robotData.PrefabID);
            var robotGo = stageManager.ObjectPoolManager.Get(robotAddress);
            robotGo.transform.parent = null;
            robotGo.transform.position = mine.GetSpawnPoint(i).position;
            robotControllers[i] = robotGo.GetComponent<MiningRobotController>();
            robotControllers[i].Init(planetID, equipments[planetID][i].miningRobotId, i);
            robotControllers[i].SetOreStorage(mine.GetOre(i), mine.GetStorage(i));
        }
    }

    private void OnEquipChanged(int planetID)
    {
        if (!IsActive)
        {
            return;
        }

        if (Variables.planetMiningID != planetID)
        {
            return;
        }

        var equipments = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet;

        if (!equipments.ContainsKey(planetID))
        {
            return;
        }

        for (int i = 0; i < equipments[planetID].Length; ++i)
        {
            bool robotCreated = robotControllers[i] is not null;
            if (equipments[planetID][i].isEmpty)
            {
                if (robotCreated)
                {
                    robotControllers[i].Release();
                    robotControllers[i] = null;
                }
                continue;
            }
            if (robotCreated && robotControllers[i].RobotData.ID == equipments[planetID][i].miningRobotId)
            {
                continue;
            }
            if (robotCreated)
            {
                robotControllers[i].Release();
                robotControllers[i] = null;
            }
            var robotData = DataTableManager.RobotTable.GetData(equipments[planetID][i].miningRobotId);
            var robotAddress = DataTableManager.AddressTable.GetData(robotData.PrefabID);
            var robotGo = stageManager.ObjectPoolManager.Get(robotAddress);
            robotGo.transform.parent = null;
            robotGo.transform.position = mine.GetSpawnPoint(i).position;
            robotControllers[i] = robotGo.GetComponent<MiningRobotController>();
            robotControllers[i].Init(planetID, equipments[planetID][i].miningRobotId, i);
            robotControllers[i].SetOreStorage(mine.GetOre(i), mine.GetStorage(i));
        }
    }

    public void StartMineBattle()
    {
        status = Status.Battle;
        stageManager.StageUiManager.curtain.SetFade(true);

        stageManager.StageUiManager.IngameUIManager.mineBattleButton.gameObject.SetActive(false);

        var datas = DataTableManager.MiningBattleTable.GetDatas(Variables.planetMiningID);

        battleData = datas[Variables.planetMiningStage - 1];
        battleSpawnData = DataTableManager.MiningBattleSpawnTable.GetData(battleData.SpawnTableID);

        stageStartTime = Time.time;
        stageEndTime = stageStartTime + battleData.LimitTime;
        weight[1] = 1f;
        stageManager.StageUiManager.ResourceRow.SetActive(false);
        stageManager.StageUiManager.IngameUIManager.miningBattleCenterHpBar.gameObject.SetActive(true);
        stageManager.StageUiManager.IngameUIManager.miningBattleTimerGameObject.SetActive(true);
        stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(false);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpInactive(0);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpInactive(1);

        centerHP = battleData.HitCount;
        //TODO: ��Ʈ�� ���̺� �־����
        stageManager.StageUiManager.IngameUIManager.miningBattleCenterHpBar.value = 1f;

        for (int i = 0; i < battleSpawnData.SpawnerActivationTimes.Length; ++i)
        {
            spawnTimers[i] = Time.time + battleSpawnData.SpawnerActivationTimes[i];
        }

        spawnIntervalReduceTime = Time.time + ChangeInterval;
        spawnInterval = battleSpawnData.SpawnInterval;

        var minego = stageManager.ObjectPoolManager.Get(DataTableManager.AddressTable.GetData(battleData.PrefabID));
        minego.transform.SetParent(null);
        mineDefence = minego.GetComponent<MineDefence>();
        mineDefence.MonsterGoals[0].GetComponent<AttackedEvent>().OnAttacked += OnAttacked;
        mineDefence.MonsterGoals[1].GetComponent<AttackedEvent>().OnAttacked += OnAttacked;

        stageManager.UnitPartyManager.UnitSpawn(mineDefence.UnitSpawnPoints);

        for (UnitTypes type = UnitTypes.Tanker; type <= UnitTypes.Healer; ++type)
        {
            stageManager.UnitPartyManager.GetUnit(type).transform.forward = Vector3.back;
        }

        SoundManager.Instance.PlayBGM("DungeonBGM");
        stageManager.UnitPartyManager.GetUnit(UnitTypes.Tanker).GetComponent<UnitStats>().range = 5f;
    }

    public void UpdateTimer(float currentTime)
    {
        remainingTime = stageEndTime - currentTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            OnStageEnd(true);
            return;
        }

        if (stageManager.UnitPartyManager.UnitCount == 0)
        {
            OnStageEnd(false);
            return;
        }

        if (spawnIntervalReduceTime < currentTime)
        {
            spawnIntervalReduceTime += ChangeInterval;
            spawnInterval -= battleSpawnData.SpawnIntervalReduction;
            weight[1] += battleSpawnData.HPIncreasement;
            stageManager.StageMonsterManager.SetWeight(weight);
        }

        for (int i = 0; i < spawnTimers.Length; ++i)
        {
            if (battleSpawnData.SpawnerActivationTimes[i] < 0f)
            {
                continue;
            }
            if (currentTime > spawnTimers[i])
            {
                spawnTimers[i] += spawnInterval;
                var monsterController = stageManager.StageMonsterManager.Spawn(mineDefence.MonsterSpawnPoints[i].position, battleSpawnData.SpawnMonsterIDs[i]);
                monsterController.SetTarget(mineDefence.MonsterGoals[i / 2]);
                monsterController.hasTarget = true;
            }
        }
        stageManager.StageUiManager.IngameUIManager.miningBattleTimerText.SetString(Defines.DirectStringID, remainingTime.ToString("F2"));
    }

    private void OnAttacked(AttackedEvent sender)
    {
        --centerHP;
        if (centerHP <= 0 && status == Status.Battle)
        {
            centerHP = 0;
            OnStageEnd(false);
        }
        //TODO: ��Ʈ�� ���̺� �־����
        SoundManager.Instance.PlaySFX("ObjectHitSFX");
        stageManager.StageUiManager.IngameUIManager.miningBattleCenterHpBar.value = (float)centerHP / battleData.HitCount;
    }

    public void OnStageEnd(bool isTimeOver)
    {
        SoundManager.Instance.StopBGM();
        status = Status.Normal;

        mineDefence.Release();

        stageManager.StageUiManager.ResourceRow.SetActive(true);
        stageManager.StageUiManager.IngameUIManager.mineBattleButton.gameObject.SetActive(true);
        stageManager.StageMonsterManager.StopMonster();
        stageManager.UnitPartyManager.UnitDespawn();
        stageManager.StageMonsterManager.ClearMonster();
        stageManager.StageUiManager.HPBarManager.ClearHPBar();

        if (isTimeOver && centerHP > 0)
        {
            List<(int itemID, BigNumber amount)> gotItems = new List<(int itemID, BigNumber amount)>();

            ItemManager.AddItem(battleData.Reward1ItemID, battleData.Reward1ItemCount);
            gotItems.Add((battleData.Reward1ItemID, battleData.Reward1ItemCount));

            for (int i = 0; i < battleData.Reward2ItemIDs.Length; ++i)
            {
                if (Random.value < battleData.Reward2ItemProbabilities[i])
                {
                    int itemCount = Random.Range(0, battleData.Reward2ItemCounts[i]) + 1;
                    ItemManager.AddItem(battleData.Reward2ItemIDs[i], itemCount);
                    gotItems.Add((battleData.Reward2ItemIDs[i], itemCount));
                }
            }

            ++SaveLoadManager.Data.mineBattleData.mineBattleCount;
            SaveLoadManager.Data.mineBattleData.lastClearTime = TimeManager.Instance.GetEstimatedServerTime();

            var datas = DataTableManager.MiningBattleTable.GetDatas(battleData.PlanetTableID);
            if (SaveLoadManager.Data.stageSaveData.ClearedMineStage[battleData.PlanetTableID] < battleData.Stage)
            {
                SaveLoadManager.Data.stageSaveData.ClearedMineStage[battleData.PlanetTableID] = battleData.Stage;
                SaveLoadManager.Data.stageSaveData.HighMineStage[battleData.PlanetTableID] = Mathf.Min(battleData.Stage + 1, datas[datas.Count - 1].Stage);
            }

            SaveLoadManager.SaveGame();

            stageManager.StageUiManager.IngameUIManager.miningBattleResultWindow.ShowClear(battleData, gotItems);
        }
        else
        {
            stageManager.StageUiManager.IngameUIManager.miningBattleResultWindow.ShowDefeat(battleData);
        }
        stageManager.StageUiManager.IngameUIManager.miningBattleTimerGameObject.SetActive(false);
        stageManager.StageUiManager.IngameUIManager.miningBattleCenterHpBar.gameObject.SetActive(false);

        stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(true);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(0);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(1);
    }
}