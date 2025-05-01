using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonStageStatusMachine : StageStatusMachine
{
    protected enum Status
    {
        Play,
        Clear,
        Timeout,
        Defeat,
    }

    protected DungeonTable.Data dungeonData;

    protected int currentType;
    protected int currentStage;
    protected int currentWave;

    protected WaveTable.Data waveData;

    protected DungeonStageStatusMachineData stageMachineData;

    private Status status;

    protected float stepTimer;

    protected float remainingTime;

    protected event System.Action onStageEnd;

    public DungeonStageStatusMachine(StageManager stageManager) : base(stageManager)
    {
        onStageEnd += stageManager.StageEnd;
    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (DungeonStageStatusMachineData)stageMachineData;
    }

    public override void Start()
    {
        SetStageText();
        SetDungeonData();
        InstantiateBackground();
        UnitSpawn();
        if (dungeonData.KeyPoint == 0)
        {
            ItemManager.ConsumeItem(dungeonData.NeedKeyItemID, dungeonData.NeedKeyItemCount);
        }

        stageManager.CameraManager.SetCameraOffset();
        NextWave(true);
        stageManager.StageMonsterManager.OnMonsterCleared += OnMonsterCleared;
    }

    public override void Update()
    {
        float currentTime = Time.time;

        switch (status)
        {
            case Status.Play:
                UpdateTimer(currentTime);
                break;
        }
    }

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        if (IsActive)
        {
            Start();
        }
        else
        {
            Time.timeScale = 1f;
            stageManager.ReleaseBackground();

            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;
            stageManager.StageMonsterManager.StopMonster();
            stageManager.UnitPartyManager.UnitDespawn();
            stageManager.StageMonsterManager.ClearMonster();

            var prefabID = DataTableManager.AddressTable.GetData(dungeonData.PrefabID);
        }
    }

    protected void UpdateTimer(float currentTime)
    {
        remainingTime = stageEndTime - currentTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            OnStageEnd(Status.Timeout);
        }
        if (stageManager.UnitPartyManager.UnitCount == 0)
        {
            OnStageEnd(Status.Defeat);
        }

        stageManager.StageUiManager.IngameUIManager.SetTimer(remainingTime);
    }

    protected void NextWave(bool isFirstWave)
    {
        stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);

        status = Status.Play;

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.CorpsIDs[currentWave - 1]);

        if (corpsData is null)
        {
            Exit();
        }

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();
        if (unit != null)
        {
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * waveData.RespawnDistance, corpsData);
        }
        else
        {
            stageManager.StageMonsterManager.Spawn(Vector3.zero, corpsData);
        }

        stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);
        ++currentWave;
    }

    protected void InstantiateBackground()
    {
        var prefabID = DataTableManager.AddressTable.GetData(dungeonData.PrefabID);
        var background = stageManager.ObjectPoolManager.Get(prefabID);
        background.transform.parent = null;
        background.transform.position = Vector3.back * 30f;
        background.transform.rotation = Quaternion.identity;
    }

    public override void Exit()
    {
        if (dungeonData.Type == 2 && remainingTime > 0f)
        {
            remainingTime = 0f;
            OnStageEnd(Status.Timeout);
            return;
        }

        stageManager.SetStatus(IngameStatus.Planet);
    }

    protected void OnMonsterCleared()
    {
        if (currentWave > waveData.CorpsIDs.Length)
        {
            OnStageEnd(Status.Clear);
            return;
        }

        NextWave(false);
    }

    protected void OnStageEnd(Status status)
    {
        this.status = status;
        Time.timeScale = 0f;
        stageManager.StageUiManager.curtain.gameObject.SetActive(false);
        onStageEnd?.Invoke();

        switch (this.status)
        {
            case Status.Clear:
                bool firstCleared = SaveLoadManager.Data.stageSaveData.clearedDungeon[currentType] < currentStage;

                if (firstCleared)
                {
                    SaveLoadManager.Data.stageSaveData.clearedDungeon[currentType] = currentStage;

                    if (DataTableManager.DungeonTable.CountOfStage(currentType) > currentStage)
                    {
                        SaveLoadManager.Data.stageSaveData.highestDungeon[currentType] = currentStage + 1;
                    }
                    else
                    {
                        SaveLoadManager.Data.stageSaveData.highestDungeon[currentType] = currentStage;
                    }

                    GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.DungeonClear);
                    ItemManager.AddItem(dungeonData.RewardItemID, dungeonData.FirstClearRewardItemCount);
                }
                else
                {
                    ItemManager.AddItem(dungeonData.RewardItemID, dungeonData.ClearRewardItemCount);
                }
                if (dungeonData.KeyPoint == 1)
                {
                    ItemManager.ConsumeItem(dungeonData.NeedKeyItemID, dungeonData.NeedKeyItemCount);
                }
                stageManager.StageUiManager.IngameUIManager.DungeonExitConfirmWindow.gameObject.SetActive(false);
                stageManager.StageUiManager.IngameUIManager.DungeonEndWindow.Open(true, firstCleared);

                break;
            case Status.Timeout:
                if (dungeonData.Type == 1)
                {
                    stageManager.StageUiManager.IngameUIManager.DungeonExitConfirmWindow.gameObject.SetActive(false);
                    stageManager.StageUiManager.IngameUIManager.DungeonEndWindow.Open(false, false);
                }
                else if (dungeonData.Type == 2)
                {
                    Dungeon2End();
                }
                break;
            case Status.Defeat:
                stageManager.StageUiManager.IngameUIManager.DungeonExitConfirmWindow.gameObject.SetActive(false);
                stageManager.StageUiManager.IngameUIManager.DungeonEndWindow.Open(false, false);
                break;
        }

        SaveLoadManager.SaveGame();
    }

    protected void SetStageText()
    {
        currentType = Variables.currentDungeonType;
        currentStage = Variables.currentDungeonStage;
        currentWave = 1;
    }

    protected void SetDungeonData()
    {
        dungeonData = DataTableManager.DungeonTable.GetData(currentType, currentStage);
        waveData = DataTableManager.WaveTable.GetData(dungeonData.WaveID);
        stageEndTime = Time.time + dungeonData.LimitTime;
        stageManager.StageUiManager.IngameUIManager.SetDungeonStageText(dungeonData.Type, dungeonData.Stage);

        if (waveData.CorpsIDs.Length > 1)
        {
            stageManager.StageUiManager.IngameUIManager.waveText.gameObject.SetActive(true);
            stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);
        }
        else
        {
            stageManager.StageUiManager.IngameUIManager.waveText.gameObject.SetActive(false);
        }
    }

    protected void UnitSpawn()
    {
        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.UnitPartyManager.ResetUnitHealth();
        stageManager.UnitPartyManager.ResetSkillCoolTime();
        stageManager.UnitPartyManager.ResetStatus();
    }

    public override void Reset()
    {
        stageManager.ReleaseDamageTexts();
        SetStageText();
        SetDungeonData();

        Time.timeScale = 1f;
        stageManager.StageMonsterManager.ClearMonster();

        stageManager.ReleaseBackground();
        InstantiateBackground();
        UnitSpawn();

        stageManager.CameraManager.SetCameraOffset();
        NextWave(true);
    }

    private void Dungeon2End()
    {
        var boss = stageManager.StageMonsterManager.GetMonsters(1)[0].GetComponent<MonsterStats>();
        var damage = -boss.Hp;
        if (SaveLoadManager.Data.stageSaveData.dungeonTwoDamage < damage)
        {
            SaveLoadManager.Data.stageSaveData.dungeonTwoDamage = damage;
        }
        var rewardsData = DataTableManager.DamageDungeonRewardTable.GetRewards(damage);
        if (rewardsData.Count > 0)
        {
            foreach (var reward in rewardsData)
            {
                ItemManager.AddItem(reward.Key, reward.Value);
            }
            if (SaveLoadManager.Data.stageSaveData.clearedDungeon[currentType] < currentStage)
            {
                SaveLoadManager.Data.stageSaveData.clearedDungeon[currentType] = currentStage;
            }
            stageManager.StageUiManager.IngameUIManager.DungeonExitConfirmWindow.gameObject.SetActive(false);
            stageManager.StageUiManager.IngameUIManager.DamageDungeonEndWindow.Open(damage, rewardsData);
        }
        else
        {
            stageManager.StageUiManager.IngameUIManager.DungeonExitConfirmWindow.gameObject.SetActive(false);
            stageManager.StageUiManager.IngameUIManager.DamageDungeonEndWindow.Open(damage);
        }
    }
}