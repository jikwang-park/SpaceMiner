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

    protected float spawnDistance = 10f;

    protected WaveTable.Data waveData;

    protected bool cleared = false;

    public DungeonStageStatusMachine(StageManager stageManager) : base(stageManager)
    {

    }

    public override void Start()
    {
        InitStage();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.CameraManager.ResetCameraPosition();
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
            stageManager.ObjectPoolManager.Clear(dungeonData.PrefabID);
        }
    }

    protected IEnumerator CoSpawnNextWave(float delay = 0.5f)
    {
        stageManager.StageUiManager.IngameUIManager.SetWaveText(currentWave);
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[currentWave - 1]);

        if (corpsData is null)
        {
            Exit();
            yield break;
        }

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();
        if (unit != null)
        {
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * spawnDistance, corpsData);
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
        var background = stageManager.ObjectPoolManager.Get(dungeonData.PrefabID);
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
        if (currentWave > waveData.WaveCorpsIDs.Length)
        {
            cleared = true;
            OnStageClear();
            return;
        }

        stageManager.StartCoroutine(CoSpawnNextWave());
    }

    protected void OnStageClear()
    {
        if (SaveLoadManager.Data.stageSaveData.highestDungeon[currentType] == currentStage
            && DataTableManager.DungeonTable.CountOfStage(currentType) > currentStage)
        {
            ++SaveLoadManager.Data.stageSaveData.highestDungeon[currentType];
        }
        ItemManager.AddItem(dungeonData.ItemID, dungeonData.ClearReward);
        ItemManager.ConsumeItem(dungeonData.DungeonKeyID, dungeonData.KeyCount);
        SaveLoadManager.SaveGame();

        stageManager.StageUiManager.IngameUIManager.OpenDungeonEndWindow("Clear", true);
    }

    protected void InitStage()
    {
        currentType = Variables.currentDungeonType;
        currentStage = Variables.currentDungeonStage;
        currentWave = 1;

        dungeonData = DataTableManager.DungeonTable.GetData(currentType, currentStage);
        waveData = DataTableManager.WaveTable.GetData(dungeonData.WaveCorpsID);

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
        stageManager.CameraManager.ResetCameraPosition();
        stageManager.StartCoroutine(CoSpawnNextWave());
    }
}
