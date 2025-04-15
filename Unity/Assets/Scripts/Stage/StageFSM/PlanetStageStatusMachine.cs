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

    protected WaitForSeconds wait1s = new WaitForSeconds(1f);

    protected bool cleared = false;

    private StageSaveData stageLoadData = SaveLoadManager.Data.stageSaveData;

    private PlanetStageStatusMachineData stageMachineData;

    public PlanetStageStatusMachine(StageManager stageManager) : base(stageManager)
    {

    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (PlanetStageStatusMachineData)stageMachineData;
    }


    public override void Start()
    {
        stageManager.StageUiManager.IngameUIManager.SetGoldText();

        InitStageInfo();
        InstantiateBackground();

        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.UnitPartyManager.ResetSkillCoolTime();
        stageManager.CameraManager.SetCameraOffset();
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
        if (stageManager.UnitPartyManager.UnitCount == 0)
        {
            EndStage(false);
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

            var prefabID = DataTableManager.AddressTable.GetData(stageData.PrefabID);
            stageManager.ObjectPoolManager.Clear(prefabID);
        }
    }

    protected IEnumerator SpawnNextWave(float delay = 0.5f)
    {
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.CorpsIDs[CurrentWave - 1]);

        if (corpsData is null)
        {
            EndStage(false);
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

        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
        ++CurrentWave;
    }

    protected void InstantiateBackground()
    {
        var prefabID = DataTableManager.AddressTable.GetData(stageData.PrefabID);
        var background = stageManager.ObjectPoolManager.Get(prefabID);
        background.transform.parent = null;
        background.transform.position = Vector3.back * 30f;
        background.transform.rotation = Quaternion.identity;
    }

    protected void OnMonsterCleared()
    {
        if (CurrentWave > waveData.CorpsIDs.Length)
        {
            cleared = true;
            EndStage(true);
            return;
        }

        stageManager.StartCoroutine(SpawnNextWave());
    }

    protected void OnMonsterDie()
    {
        ++SaveLoadManager.Data.questProgressData.monsterCount;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.Exterminate);
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

        if ((CurrentPlanet == stageLoadData.clearedPlanet && CurrentStage > stageLoadData.clearedStage)
            || (CurrentPlanet > stageLoadData.clearedPlanet))
        {
            stageLoadData.clearedPlanet = CurrentPlanet;
            stageLoadData.clearedStage = CurrentStage;

            if (stageData.FirstClearRewardID != 0)
            {
                ItemManager.AddItem(stageData.FirstClearRewardID, stageData.FirstClearRewardCount);
            }

            stageManager.StageUiManager.IngameUIManager.SetGoldText();

            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                stageLoadData.highPlanet = CurrentPlanet;
                stageLoadData.highStage = CurrentStage + 1;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                stageLoadData.highPlanet = CurrentPlanet + 1;
                stageLoadData.highStage = 1;
            }
            else
            {
                stageLoadData.highPlanet = CurrentPlanet;
                stageLoadData.highStage = CurrentStage;
            }

            if (stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.activeSelf)
            {
                stageManager.StageUiManager.IngameUIManager.StageSelectWindow.RefreshStageWindow();
            }

            GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StageClear);
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

                if (stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.activeSelf)
                {
                    stageManager.StageUiManager.IngameUIManager.StageSelectWindow.RefreshStageWindow();
                }
                stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();
                Reset();
                yield break;
                //SceneManager.LoadScene(0);
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
        waveData = DataTableManager.WaveTable.GetData(stageData.WaveID);
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
        stageManager.StopAllCoroutines();
        stageManager.StageMonsterManager.ClearMonster();
        stageManager.StageUiManager.IngameUIManager.SetGoldText();

        InitStageInfo();
        if (previousPlanet != CurrentPlanet)
        {
            stageManager.ReleaseBackground();

            InstantiateBackground();

            stageManager.UnitPartyManager.UnitSpawn();
            stageManager.CameraManager.SetCameraOffset();
        }

        stageManager.StartCoroutine(SpawnNextWave());
        stageManager.StageUiManager.curtain.SetFade(false);
    }
}
