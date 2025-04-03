using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetStageStatusMachine : StageStatusMachine
{
    protected int CurrentPlanet;
    protected int CurrentStage;
    protected int CurrentWave;

    protected StageTable.Data stageData;

    protected WaveTable.Data waveData;

    public float spawnDistance = 10f;

    protected Coroutine coroutine;

    protected WaitForSeconds wait1s = new WaitForSeconds(1f);

    protected bool cleared = false;

    private StageSaveData stageLoadData = SaveLoadManager.Data.stageSaveData;

    public PlanetStageStatusMachine(StageManager stageManager) : base(stageManager)
    {

    }

    public override void Start()
    {
        stageManager.StageUiManager.SetGoldText();

        InitStage();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.StartCoroutine(CoSpawnNextWave());
        stageManager.StageMonsterManager.OnMonsterDie += OnMonsterDie;
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
                ResetStage(false);
            }
        }

        stageManager.StageUiManager.SetTimer(remainTime);
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
            stageManager.StageMonsterManager.OnMonsterDie -= OnMonsterDie;
            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;
            stageManager.UnitPartyManager.UnitDespawn();
            stageManager.StageMonsterManager.ClearMonster();
        }
    }

    protected IEnumerator CoSpawnNextWave(float delay = 2f)
    {
        stageManager.StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[CurrentWave - 1]);

        if (corpsData is null)
        {
            ResetStage(false);
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

        stageManager.StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
        ++CurrentWave;
    }

    protected void InstantiateBackground()
    {
        var background = stageManager.objectPoolManager.Get(stageData.PrefabId);
        background.transform.parent = null;
        background.transform.position = Vector3.back * 30f;
        background.transform.rotation = Quaternion.identity;
    }

    protected void OnMonsterCleared()
    {
        if (CurrentWave > waveData.WaveCorpsIDs.Length)
        {
            cleared = true;
            ResetStage(true);
            return;
        }

        stageManager.StartCoroutine(CoSpawnNextWave());
    }

    protected void OnMonsterDie()
    {
        stageManager.StageUiManager.SetGoldText();
    }

    protected void ResetStage(bool cleared)
    {
        if (!cleared)
        {
            stageManager.StartCoroutine(CoStageLoad());
            return;
        }

        stageManager.StartCoroutine(CoClearStage());
    }

    private IEnumerator CoClearStage()
    {
        stageManager.StageUiManager.SetStageMessage(true);
        stageManager.StageUiManager.SetActiveStageMessage(true);

        if (CurrentPlanet == stageLoadData.highPlanet
            && CurrentStage == stageLoadData.highStage)
        {
            if (stageData.FirstClearRewardID != 0)
            {
                ItemManager.AddItem(stageData.FirstClearRewardID, stageData.FirstClearRewardCount);
            }
            stageManager.StageUiManager.SetGoldText();

            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                stageLoadData.highStage = CurrentStage + 1;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                stageLoadData.highPlanet = CurrentPlanet + 1;
                stageLoadData.highStage = 1;
            }

            stageManager.StageUiManager.UnlockStage(stageLoadData.highPlanet, stageLoadData.highStage);
        }

        yield return wait1s;


        if (Variables.stageMode == StageMode.Ascend)
        {
            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                ++stageLoadData.currentStage;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                ++stageLoadData.currentPlanet;
                stageLoadData.currentStage = 1;

                SaveLoadManager.SaveGame();
                SceneManager.LoadScene(0);
            }
        }

        InitStage();
        stageManager.StartCoroutine(CoSpawnNextWave());

        SaveLoadManager.SaveGame();

        stageManager.StageUiManager.SetActiveStageMessage(false);
    }

    private IEnumerator CoStageLoad()
    {
        if (stageLoadData.currentStage > 1)
        {
            --stageLoadData.currentStage;
        }

        Variables.stageMode = StageMode.Repeat;
        stageManager.StageUiManager.SetStageMessage(false);
        stageManager.StageUiManager.SetActiveStageMessage(true);
        SaveLoadManager.SaveGame();

        yield return wait1s;

        SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }

    protected void InitStage()
    {
        CurrentPlanet = stageLoadData.currentPlanet;
        CurrentStage = stageLoadData.currentStage;
        CurrentWave = 1;
        stageEndTime = Time.time + 60f;
        cleared = false;

        //stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentPlanet, CurrentStage));
        stageData = DataTableManager.StageTable.GetStageData(CurrentPlanet, CurrentStage);
        waveData = DataTableManager.WaveTable.GetData(stageData.CorpsID);
        stageManager.StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
    }
}
