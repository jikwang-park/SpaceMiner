using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [field: SerializeField]
    private int CurrentPlanet { get; set; }
    [field: SerializeField]
    private int CurrentStage { get; set; }
    [field: SerializeField]
    private int CurrentWave { get; set; }

    public StageMonsterManager StageMonsterManager { get; private set; }

    public UnitPartyManager UnitPartyManager { get; private set; }

    [field: SerializeField]
    public StageUiManager StageUiManager { get; private set; }

    private WaveTable.Data waveData;

    private float stageEndTime;

    [SerializeField]
    private float spawnDistance = 10f;


    private StageTable.Data stageData;

    private WaitForSeconds wait1 = new WaitForSeconds(1f);

    private void Awake()
    {
        StageMonsterManager = GetComponent<StageMonsterManager>();
        StageMonsterManager.OnMonsterDie += OnMonsterDie;
        StageMonsterManager.OnMonsterCleared += OnMonsterCleared;

        UnitPartyManager = GetComponent<UnitPartyManager>();
        
        SaveLoadManager.LoadGame();
        DoLoad();
    }

    private void Start()
    {
        SaveLoadManager.onSaveRequested += DoSave;


        StageUiManager.SetGoldText();

        SetStageInfo();

        var background = GetComponent<ObjectPoolManager>().gameObjectPool[stageData.PrefabId].Get();
        background.transform.parent = null;
        background.transform.position = Vector3.back * 30f;
        background.transform.rotation = Quaternion.identity;

        //Addressables.InstantiateAsync(stage, Vector3.back * 30f, Quaternion.identity);
        SpawnNextWave();
    }

    private void Update()
    {
        float remainTime = stageEndTime - Time.time;

        if (remainTime <= 0f)
        {
            remainTime = 0f;
            OnTimeOver();
        }

        StageUiManager.SetTimer(remainTime);
    }


    private void SpawnNextWave(float delay = 2f)
    {
        StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);

        StartCoroutine(coSpawnNextWave(delay));
    }


    private void OnMonsterDie()
    {
        StageUiManager.SetGoldText();
    }


    private IEnumerator coSpawnNextWave(float delay)
    {
        yield return new WaitForSeconds(delay);

        var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[CurrentWave - 1]);

        if (corpsData is null)
        {
            ResetStage(false);
        }

        Transform unit = UnitPartyManager.GetFirstLineUnitTransform();
        if (unit != null)
        {
            StageMonsterManager.Spawn(unit.position + Vector3.forward * spawnDistance, corpsData);
        }
        else
        {
            StageMonsterManager.Spawn(Vector3.zero, corpsData);
        }

        StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
        ++CurrentWave;
    }




    private void SetStageInfo()
    {
        CurrentPlanet = Variables.planetNumber;
        CurrentStage = Variables.stageNumber;
        CurrentWave = 1;
        stageEndTime = Time.time + 60f;

        //stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentPlanet, CurrentStage));
        stageData = DataTableManager.StageTable.GetData(CurrentPlanet * 100000 + CurrentStage * 100);
        waveData = DataTableManager.WaveTable.GetData(stageData.CorpsID);
        StageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
    }


    private void OnMonsterCleared()
    {
        if (CurrentWave > waveData.WaveCorpsIDs.Length)
        {
            ResetStage(true);
            return;
        }

        SpawnNextWave();
    }
    private void ResetStage(bool cleared)
    {
        if (!cleared)
        {
            StartCoroutine(CoStageLoad());
            return;
        }

        StartCoroutine(CoClearStage());
    }
    private IEnumerator CoClearStage()
    {
        StageUiManager.SetStageMessage(true);
        StageUiManager.SetActiveStageMessage(true);

        if (CurrentPlanet == Variables.maxPlanetNumber
            && CurrentStage == Variables.maxStageNumber)
        {
            if (stageData.FirstClearRewardID != 0)
            {
                ItemManager.AddItem(stageData.FirstClearRewardID, stageData.FirstClearRewardCount);
            }
            StageUiManager.SetGoldText();

            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                Variables.maxStageNumber = CurrentStage + 1;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                Variables.maxPlanetNumber = CurrentPlanet + 1;
                Variables.maxStageNumber = 1;
            }

            StageUiManager.UnlockStage(Variables.maxPlanetNumber, Variables.maxStageNumber);
        }

        yield return wait1;


        if (Variables.stageMode == StageMode.Ascend)
        {
            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                ++Variables.stageNumber;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                ++Variables.planetNumber;
                Variables.stageNumber = 1;

                SaveLoadManager.SaveGame();
                SceneManager.LoadScene(0);
            }
        }

        SetStageInfo();
        SpawnNextWave();

        SaveLoadManager.SaveGame();

        StageUiManager.SetActiveStageMessage(false);
    }

    private IEnumerator CoStageLoad()
    {
        if (Variables.stageNumber > 1)
        {
            --Variables.stageNumber;
        }

        Variables.stageMode = StageMode.Repeat;
        StageUiManager.SetStageMessage(false);
        StageUiManager.SetActiveStageMessage(true);
        SaveLoadManager.SaveGame();

        yield return wait1;

        SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }

    private void OnTimeOver()
    {
        ResetStage(false);
    }

    private void DoSave(TotalSaveData totalSaveData)
    {
        StageSaveData stageSaveData = new StageSaveData
        {
            currentPlanet = Variables.planetNumber,
            currentStage = Variables.stageNumber,
            highPlanet = Variables.maxPlanetNumber,
            highStage = Variables.maxStageNumber,
        };
        totalSaveData.stageSaveData = stageSaveData;
    }

    private void DoLoad()
    {
        if (SaveLoadManager.LoadedData == null)
        {
            Debug.Log("ÀúÀåµÈ µ¥ÀÌÅÍ°¡ ¾ø½À´Ï´Ù. ±âº» °ªÀ¸·Î ÁøÇàÇÕ´Ï´Ù.");
            return;
        }

        StageSaveData stageLoadData = SaveLoadManager.LoadedData.stageSaveData;

        if (stageLoadData != null)
        {
            Variables.planetNumber = stageLoadData.currentPlanet;
            Variables.stageNumber = stageLoadData.currentStage;
            Variables.maxPlanetNumber = stageLoadData.highPlanet;
            Variables.maxStageNumber = stageLoadData.highStage;
            // ï¿½Ö°ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½à¼º ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ò·ï¿½ï¿½ï¿½ï¿½ï¿½
            // ï¿½Ö°ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ò·ï¿½ï¿½ï¿½ï¿½ï¿½
        }
    }
}
