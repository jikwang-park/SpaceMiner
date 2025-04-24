using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetStageStatusMachine : StageStatusMachine
{
    protected enum Status
    {
        Play,
        Clear,
        ClearPlanet,
        Timeout,
        Defeat,
        CorpsNotFound,
    }

    protected int CurrentPlanet;
    protected int CurrentStage;
    protected int CurrentWave;

    protected StageTable.Data stageData;

    protected WaveTable.Data waveData;

    private StageSaveData stageLoadData = SaveLoadManager.Data.stageSaveData;

    private PlanetStageStatusMachineData stageMachineData;

    private Status status;

    protected float remainingTime;

    protected event System.Action onStageEnd;

    public PlanetStageStatusMachine(StageManager stageManager) : base(stageManager)
    {
        onStageEnd += stageManager.StageEnd;
    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (PlanetStageStatusMachineData)stageMachineData;
    }

    public override void Start()
    {
        SetStageText();
        SetStageData();
        InstantiateBackground();
        UnitSpawn();

        stageManager.CameraManager.SetCameraOffset();
        NextWave();
        SetEvent(true);
    }


    public override void Update()
    {
        float currentTime = Time.time;
        switch (status)
        {
            case Status.Play:
                UpdateTimer(currentTime);
                break;
            case Status.Clear:
            case Status.ClearPlanet:
            case Status.Timeout:
            case Status.Defeat:
                NextStage();
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
            stageManager.ReleaseBackground();
            stageManager.StageUiManager.IngameUIManager.CloseStageEndWindow();
            stageManager.StageMonsterManager.SetWeight(1f);
            SetEvent(false);
            ClearStage();
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

    protected void NextWave()
    {
        stageManager.UnitPartyManager.ResetStatus();
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);

        status = Status.Play;

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.CorpsIDs[CurrentWave - 1]);

        if (corpsData is null)
        {
            OnStageEnd(Status.Defeat);
        }

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();

        if (unit is not null)
        {
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * waveData.RespawnDistance, corpsData);
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
            OnStageEnd(Status.Clear);
            return;
        }
        NextWave();
    }

    protected void OnStageEnd(Status status)
    {
        this.status = status;
        onStageEnd?.Invoke();

        switch (this.status)
        {
            case Status.Clear:
                stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Clear", stageMachineData.stageEndWindowDuration);

                bool ClearedStageChanged = CheckClearedStageChange();
                if (ClearedStageChanged && stageData.FirstClearRewardID != 0)
                {
                    GetFirstReward();
                }
                if (Variables.stageMode == StageMode.Ascend)
                {
                    CheckPlanetClear();
                }
                break;
            case Status.Timeout:
                stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Time Over", stageMachineData.stageEndWindowDuration);
                FailStageSet();
                break;
            case Status.Defeat:
                stageManager.StageUiManager.IngameUIManager.OpenStageEndWindow("Defeat", stageMachineData.stageEndWindowDuration);
                FailStageSet();
                break;
        }
        SaveLoadManager.SaveGame();
    }

    protected void NextStage()
    {

        if (status == Status.ClearPlanet
            || status == Status.Defeat
            || status == Status.Timeout
            || stageManager.UnitPartyManager.UnitCount != 3)
        {
            Reset();
        }
        else
        {
            stageManager.UnitPartyManager.ResetUnitHealth();
            stageManager.UnitPartyManager.ResetSkillCoolTime();
            stageManager.UnitPartyManager.ResetStatus();

            SetStageText();
            SetStageData();
            NextWave();
        }
    }

    protected void FailStageSet()
    {
        if (stageLoadData.currentStage > 1)
        {
            --stageLoadData.currentStage;
        }
        stageManager.StageUiManager.IngameUIManager.RushSelectToggle.isOn = false;
    }

    protected void GetFirstReward()
    {
        ItemManager.AddItem(stageData.FirstClearRewardID, stageData.FirstClearRewardCount);

        if (stageData.FirstClearRewardID == (int)Currency.Gold)
        {
            stageManager.StageUiManager.IngameUIManager.SetGoldText();
        }
    }

    protected void OnMonsterDie()
    {
        ++SaveLoadManager.Data.questProgressData.monsterCount;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.Exterminate);
        stageManager.StageUiManager.IngameUIManager.SetGoldText();
    }

    protected bool CheckClearedStageChange()
    {
        bool renewed = (CurrentPlanet == stageLoadData.clearedPlanet && CurrentStage > stageLoadData.clearedStage)
            || (CurrentPlanet > stageLoadData.clearedPlanet);

        if (renewed)
        {
            stageLoadData.clearedPlanet = CurrentPlanet;
            stageLoadData.clearedStage = CurrentStage;

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

            if (stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.activeInHierarchy)
            {
                stageManager.StageUiManager.IngameUIManager.StageSelectWindow.RefreshStageWindow();
            }

            GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StageClear);
        }


        return renewed;
    }

    protected void CheckPlanetClear()
    {
        if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
        {
            ++stageLoadData.currentStage;
        }
        else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
        {
            status = Status.ClearPlanet;
            ++stageLoadData.currentPlanet;
            stageLoadData.currentStage = 1;

            if (stageManager.StageUiManager.IngameUIManager.StageSelectWindow.gameObject.activeSelf)
            {
                stageManager.StageUiManager.IngameUIManager.StageSelectWindow.RefreshStageWindow();
            }
        }
    }

    protected void SetStageText()
    {
        CurrentPlanet = stageLoadData.currentPlanet;
        CurrentStage = stageLoadData.currentStage;
        CurrentWave = 1;
        stageManager.StageUiManager.IngameUIManager.SetStageText(CurrentPlanet, CurrentStage);
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
    }

    protected void SetStageData()
    {
        stageEndTime = Time.time + 60f;
        stageData = DataTableManager.StageTable.GetStageData(CurrentPlanet, CurrentStage);
        stageManager.StageMonsterManager.SetWeight(stageData.Weight);
        waveData = DataTableManager.WaveTable.GetData(stageData.WaveID);
    }

    protected void UnitSpawn()
    {
        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.UnitPartyManager.ResetUnitHealth();
        stageManager.UnitPartyManager.ResetSkillCoolTime();
        stageManager.UnitPartyManager.ResetStatus();
    }

    protected void SetEvent(bool set)
    {
        if (set)
        {
            stageManager.StageMonsterManager.OnMonsterDie += OnMonsterDie;
            stageManager.StageMonsterManager.OnMonsterCleared += OnMonsterCleared;
        }
        else
        {
            stageManager.StageMonsterManager.OnMonsterDie -= OnMonsterDie;
            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;
        }
    }

    public override void Exit()
    {
        Reset();
    }

    protected void ClearStage()
    {
        stageManager.StageMonsterManager.StopMonster();
        stageManager.UnitPartyManager.UnitDespawn();
        stageManager.StageMonsterManager.ClearMonster();
    }

    public override void Reset()
    {
        stageManager.ReleaseDamageTexts();
        stageManager.StageUiManager.curtain.SetFade(true);
        int previousPlanet = CurrentPlanet;

        ClearStage();
        SetStageText();
        SetStageData();

        stageManager.ReleaseBackground();
        InstantiateBackground();

        UnitSpawn();
        stageManager.CameraManager.SetCameraOffset();
        stageManager.StageUiManager.curtain.SetFade(false);
        NextWave();
    }
}
