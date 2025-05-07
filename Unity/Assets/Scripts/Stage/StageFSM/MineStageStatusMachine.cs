using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MineStageStatusMachine : StageStatusMachine
{
    private const float spawnIntervalReduction = 10f;

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
    private float[] spawnActivationTime = new float[4];

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

            stageManager.StageUiManager.IngameUIManager.timerText.gameObject.SetActive(false);
            stageManager.StageUiManager.IngameUIManager.waveText.gameObject.SetActive(false);

            MiningRobotInventoryManager.onEquipRobot += OnEquipChanged;
        }
        else
        {
            stageManager.CameraManager.enabled = true;
            stageManager.CameraManager.SetCameraRotation();
            stageManager.CameraManager.SetCameraOffset();

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
        stageManager.SetStatus(IngameStatus.Planet);
    }

    public override void Reset()
    {
        if (Variables.planetMiningID == planetID)
        {
            return;
        }
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
        battleData = DataTableManager.MiningBattleTable.GetData(101);
        battleSpawnData = DataTableManager.MiningBattleSpawnTable.GetData(battleData.SpawnTableID);

        stageStartTime = Time.time;
        stageEndTime = stageStartTime + battleData.LimitTime;

        stageManager.StageUiManager.IngameUIManager.waveText.gameObject.SetActive(true);
        stageManager.StageUiManager.IngameUIManager.timerText.gameObject.SetActive(true);
        stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(false);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpInactive(0);

        centerHP = battleData.HitCount;
        stageManager.StageUiManager.IngameUIManager.waveText.text = $"HP : {centerHP}";

        for (int i = 0; i < battleSpawnData.SpawnerActivationTimes.Length; ++i)
        {
            spawnTimers[i] = Time.time + battleSpawnData.SpawnerActivationTimes[i];
        }

        spawnIntervalReduceTime = Time.time + spawnIntervalReduction;
        spawnInterval = battleSpawnData.SpawnInterval;

        var minego = stageManager.ObjectPoolManager.Get(DataTableManager.AddressTable.GetData(battleData.PrefabID));
        minego.transform.SetParent(null);
        mineDefence = minego.GetComponent<MineDefence>();
        mineDefence.MonsterGoals[0].GetComponent<AttackedEvent>().OnAttacked += OnAttacked;
        mineDefence.MonsterGoals[1].GetComponent<AttackedEvent>().OnAttacked += OnAttacked;

        stageManager.UnitPartyManager.UnitSpawn(mineDefence.UnitSpawnPoints);

    }

    public void UpdateTimer(float currentTime)
    {
        remainingTime = stageEndTime - currentTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            OnStageEnd(true);
        }

        if (stageManager.UnitPartyManager.UnitCount == 0)
        {
            OnStageEnd(false);
        }

        if (spawnIntervalReduceTime < currentTime)
        {
            spawnIntervalReduceTime += spawnIntervalReduction;
            spawnInterval -= battleSpawnData.SpawnIntervalReduction;
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

                if (NavMesh.SamplePosition(mineDefence.MonsterSpawnPoints[i].position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
                {
                    var monsterController = stageManager.StageMonsterManager.Spawn(hit.position, battleSpawnData.SpawnMonsterIDs[i]);
                    monsterController.SetTarget(mineDefence.MonsterGoals[i % 2]);
                    monsterController.hasTarget = true;
                }
            }
        }

        stageManager.StageUiManager.IngameUIManager.SetTimer(remainingTime);
    }

    private void OnAttacked(AttackedEvent sender)
    {
        --centerHP;
        if (centerHP <= 0)
        {
            centerHP = 0;
            OnStageEnd(false);
        }
        stageManager.StageUiManager.IngameUIManager.waveText.text = $"HP : {centerHP}";
    }

    public void OnStageEnd(bool isTimeOver)
    {
        status = Status.Normal;
        if (isTimeOver && centerHP > 0)
        {
            ItemManager.AddItem(battleData.Reward1ItemID, battleData.Reward1ItemCount);
            if (Random.value < battleData.Reward2ItemProbability)
            {
                ItemManager.AddItem(battleData.Reward2ItemID, battleData.Reward2ItemCount);
            }
        }
        stageManager.StageUiManager.InteractableUIBackground.gameObject.SetActive(true);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Mine].SetPopUpActive(0);
    }
}