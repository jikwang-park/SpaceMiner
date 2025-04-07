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

    public float spawnDistance = 20f;

    protected WaitForSeconds wait1s = new WaitForSeconds(1f);

    protected bool cleared = false;

    private StageSaveData stageLoadData = SaveLoadManager.Data.stageSaveData;

    public PlanetStageStatusMachine(StageManager stageManager) : base(stageManager)
    {

    }

    public override void Start()
    {
        stageManager.StageUiManager.IngameUIManager.SetGoldText();

        InitStageInfo();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.CameraManager.ResetCameraPosition();
        stageManager.StartCoroutine(SpawnNextWave());
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
                EndStage(false);
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
            stageManager.StopAllCoroutines();
            stageManager.ReleaseBackground();
            stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();

            stageManager.StageMonsterManager.OnMonsterDie -= OnMonsterDie;
            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;

            stageManager.StageMonsterManager.StopMonster();
            stageManager.UnitPartyManager.UnitDespawn();
            stageManager.StageMonsterManager.ClearMonster();
            stageManager.ObjectPoolManager.Clear(stageData.PrefabId);
        }
    }

    protected IEnumerator SpawnNextWave(float delay = 0.5f)
    {
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[CurrentWave - 1]);

        if (corpsData is null)
        {
            EndStage(false);
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

        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
        ++CurrentWave;
    }

    protected void InstantiateBackground()
    {
        var background = stageManager.ObjectPoolManager.Get(stageData.PrefabId);
        background.transform.parent = null;
        background.transform.position = Vector3.back * 30f;
        background.transform.rotation = Quaternion.identity;
    }

    protected void OnMonsterCleared()
    {
        if (CurrentWave > waveData.WaveCorpsIDs.Length)
        {
            cleared = true;
            EndStage(true);
            return;
        }

        stageManager.StartCoroutine(SpawnNextWave());
    }

    protected void OnMonsterDie()
    {
        stageManager.StageUiManager.IngameUIManager.SetGoldText();
    }

    protected void EndStage(bool cleared)
    {
        if (!cleared)
        {
            stageManager.StartCoroutine(CoStageFail());
            return;
        }

        stageManager.StartCoroutine(CoClearStage());
    }

    private IEnumerator CoClearStage()
    {
        stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Clear");

        if (CurrentPlanet == stageLoadData.highPlanet
            && CurrentStage == stageLoadData.highStage)
        {
            if (stageData.FirstClearRewardID != 0)
            {
                ItemManager.AddItem(stageData.FirstClearRewardID, stageData.FirstClearRewardCount);
            }
            stageManager.StageUiManager.IngameUIManager.SetGoldText();

            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                stageLoadData.highStage = CurrentStage + 1;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                stageLoadData.highPlanet = CurrentPlanet + 1;
                stageLoadData.highStage = 1;
            }
        }

        yield return wait1s;


        if (Variables.stageMode == StageMode.Ascend)
        {
            if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                ++stageLoadData.currentPlanet;
                stageLoadData.currentStage = 1;


                SaveLoadManager.SaveGame();
                stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();
                Reset();
                //SceneManager.LoadScene(0);
            }
            else if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                ++stageLoadData.currentStage;
            }
        }

        InitStageInfo();
        stageManager.StartCoroutine(SpawnNextWave());

        SaveLoadManager.SaveGame();

        stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();
    }

    private IEnumerator CoStageFail()
    {
        if (stageLoadData.currentStage > 1)
        {
            --stageLoadData.currentStage;
        }

        Variables.stageMode = StageMode.Repeat;
        stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Fail");
        SaveLoadManager.SaveGame();

        yield return wait1s;


        stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();
        Reset();
        //SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }

    protected void InitStageInfo()
    {
        CurrentPlanet = stageLoadData.currentPlanet;
        CurrentStage = stageLoadData.currentStage;
        CurrentWave = 1;
        stageEndTime = Time.time + 60f;
        cleared = false;

        stageManager.UnitPartyManager.ResetUnitHealth();
        stageManager.UnitPartyManager.ResetSkillCoolTime();
        stageManager.UnitPartyManager.ResetBehaviorTree();

        //stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentPlanet, CurrentStage));
        stageData = DataTableManager.StageTable.GetStageData(CurrentPlanet, CurrentStage);
        waveData = DataTableManager.WaveTable.GetData(stageData.CorpsID);
        stageManager.StageUiManager.IngameUIManager.SetStageText(CurrentPlanet, CurrentStage);
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
    }

    public override void Exit()
    {
        Start();
    }

    public override void Reset()
    {
        stageManager.StageUiManager.curtain.SetFade(true);
        int previousPlanet = CurrentPlanet;
        var previousBackground = stageData.PrefabId;
        stageManager.StopAllCoroutines();
        stageManager.StageMonsterManager.ClearMonster();
        stageManager.StageUiManager.IngameUIManager.SetGoldText();

        InitStageInfo();
        if (previousPlanet != CurrentPlanet)
        {
            stageManager.ReleaseBackground();
            stageManager.ObjectPoolManager.Clear(stageData.PrefabId);
            InstantiateBackground();

            stageManager.UnitPartyManager.UnitSpawn();
            stageManager.CameraManager.ResetCameraPosition();
        }

        stageManager.StartCoroutine(SpawnNextWave());
        stageManager.StageUiManager.curtain.SetFade(false);
    }
}
