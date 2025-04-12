using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonStageStatusMachine : StageStatusMachine
{
    protected DungeonTable.Data dungeonData;

    protected int currentType;
    protected int currentStage;
    protected int currentWave;

    protected WaveTable.Data waveData;

    protected DungeonStageStatusMachineData stageMachineData;

    protected bool cleared = false;

    public DungeonStageStatusMachine(StageManager stageManager) : base(stageManager)
    {

    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (DungeonStageStatusMachineData)stageMachineData;
    }

    public override void Start()
    {
        InitStage();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.CameraManager.SetCameraOffset();
        stageManager.StartCoroutine(CoSpawnNextWave());
        stageManager.StageMonsterManager.OnMonsterCleared += OnMonsterCleared;
    }

    public override void Update()
    {
        float remainTime = stageEndTime - Time.time;

        if (remainTime <= 0f)
        {
            remainTime = 0f;
            if (!cleared)
            {
                OnTimeOver();
            }
        }

        stageManager.StageUiManager.IngameUIManager.SetTimer(remainTime);
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
            stageManager.ReleaseBackground();
            stageManager.StopAllCoroutines();

            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;
            stageManager.StageMonsterManager.StopMonster();
            stageManager.UnitPartyManager.UnitDespawn();
            stageManager.StageMonsterManager.ClearMonster();

            var prefabID = DataTableManager.AddressTable.GetData(dungeonData.PrefabID);

            stageManager.ObjectPoolManager.Clear(prefabID);
        }
    }

    protected IEnumerator CoSpawnNextWave(float delay = 0.5f)
    {
        stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.CorpsIDs[currentWave - 1]);

        if (corpsData is null)
        {
            Exit();
            yield break;
        }

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();
        if (unit != null)
        {
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * stageMachineData.spawnDistance, corpsData);
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
        stageManager.SetStatus(IngameStatus.Planet);
    }

    protected void OnTimeOver()
    {
        stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Fail");
    }

    protected void OnMonsterCleared()
    {
        if (currentWave > waveData.CorpsIDs.Length)
        {
            cleared = true;
            OnStageClear();
            return;
        }

        stageManager.StartCoroutine(CoSpawnNextWave());
    }

    protected void OnStageClear()
    {
        if (SaveLoadManager.Data.stageSaveData.clearedDungeon[currentType] < currentStage)
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
        }
        ItemManager.AddItem(dungeonData.RewardItemID, dungeonData.ClearRewardItemCount);
        ItemManager.ConsumeItem(dungeonData.NeedKeyItemID, dungeonData.NeedKeyCount);
        SaveLoadManager.SaveGame();

        stageManager.StageUiManager.IngameUIManager.OpenDungeonEndWindow("Clear", true);
    }

    protected void InitStage()
    {
        currentType = Variables.currentDungeonType;
        currentStage = Variables.currentDungeonStage;
        currentWave = 1;

        dungeonData = DataTableManager.DungeonTable.GetData(currentType, currentStage);
        waveData = DataTableManager.WaveTable.GetData(dungeonData.WaveID);

        stageEndTime = Time.time + dungeonData.LimitTime;

        stageManager.StageUiManager.IngameUIManager.SetDungeonStageText(dungeonData.Type, dungeonData.Stage);
        stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);
    }

    public override void Reset()
    {
        stageManager.StopAllCoroutines();
        stageManager.StageMonsterManager.ClearMonster();

        InitStage();
        stageManager.ReleaseBackground();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.CameraManager.SetCameraOffset();
        stageManager.StartCoroutine(CoSpawnNextWave());
    }
}
