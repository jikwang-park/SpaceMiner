using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [field: SerializeField]
    public int CurrentPlanet { get; private set; }
    [field: SerializeField]
    public int CurrentStage { get; private set; }
    [field: SerializeField]
    public int CurrentWave { get; private set; }

    public MonsterLaneManager MonsterLaneManager { get; private set; }

    public UnitPartyManager UnitPartyManager { get; private set; }

    [field: SerializeField]
    public StageUiManger stageUiManager { get; private set; }

    [SerializeField]
    private float spawnDistance = 10f;

    private WaveSpawner waveSpawner;

    private HashSet<MonsterController> monsters;

    private StageTable.Data stageData;
    private WaveTable.Data waveData;

    private float stageEndTime;

    private WaitForSeconds wait1 = new WaitForSeconds(1f);

    private BigNumber golds = 0;


    private void Awake()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        MonsterLaneManager = GetComponent<MonsterLaneManager>();
        UnitPartyManager = GetComponent<UnitPartyManager>();
        monsters = new HashSet<MonsterController>();

        SaveLoadManager.LoadGame();
    }

    private void Start()
    {
        DoLoad();

        if (SaveLoadManager.LoadedData.itemSaveData.ContainsKey((int)Currency.Gold))
        {
            stageUiManager.SetGoldText(SaveLoadManager.LoadedData.itemSaveData[(int)Currency.Gold]);
        }
        SetStageInfo();
        SaveLoadManager.onSaveRequested += DoSave;

        var background = GetComponent<ObjectPoolManager>().gameObjectPool[stageData.PrefabId].Get();

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
            ResetStage(false);
        }

        stageUiManager.SetTimer(remainTime);
    }

    public void AddMonster(MonsterController monsterController)
    {
        monsters.Add(monsterController);

        var destroyEvent = monsterController.GetComponent<DestructedDestroyEvent>();
        destroyEvent.OnDestroyed += OnMonsterDestroy;
    }

    public void SpawnNextWave(float delay = 2f)
    {
        stageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);

        StartCoroutine(coSpawnNextWave(delay));
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
            waveSpawner.Spawn(unit.position + Vector3.forward * spawnDistance, corpsData);
        else
            waveSpawner.Spawn(transform.position, corpsData);

        stageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
        ++CurrentWave;
    }

    private void OnMonsterDestroy(DestructedDestroyEvent sender)
    {
        var monsterController = sender.GetComponent<MonsterController>();
        monsters.Remove(monsterController);

        AddReward(monsterController.RewardData.Reward1, monsterController.RewardData.Count);

        stageUiManager.SetGoldText(SaveLoadManager.LoadedData.itemSaveData[monsterController.RewardData.Reward1]);

        int reward2index = monsterController.RewardData.RandomReward2();
        if (reward2index > -1)
        {
            AddReward(monsterController.RewardData.Reward2, monsterController.RewardData.counts[reward2index]);
        }

        if (monsters.Count == 0)
        {
            if (CurrentWave > waveData.WaveCorpsIDs.Length)
            {
                ResetStage(true);
                return;
            }

            SpawnNextWave();
        }
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
        stageUiManager.SetStageText(CurrentPlanet, CurrentStage, CurrentWave);
    }

    private IEnumerator coStageLoad()
    {
        if (Variables.stageNumber > 1)
        {
            --Variables.stageNumber;
        }

        Variables.stageMode = StageMode.Repeat;
        stageUiManager.SetStageMessage(false);
        stageUiManager.SetActiveStageMessage(true);
        SaveLoadManager.SaveGame();

        yield return wait1;

        SceneManager.LoadScene(0);
        //Addressables.LoadSceneAsync("StageDevelopScene");
    }

    private void ResetStage(bool cleared)
    {
        if (!cleared)
        {
            StartCoroutine(coStageLoad());
            return;
        }

        StartCoroutine(coClearStage());
    }

    private IEnumerator coClearStage()
    {
        stageUiManager.SetStageMessage(true);
        stageUiManager.SetActiveStageMessage(true);

        if (CurrentPlanet == Variables.maxPlanetNumber
            && CurrentStage == Variables.maxStageNumber)
        {
            AddReward((int)Currency.Gold, stageData.FirstClearReward);
            stageUiManager.SetGoldText(SaveLoadManager.LoadedData.itemSaveData[(int)Currency.Gold]);

            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                Variables.maxStageNumber = CurrentStage + 1;
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                Variables.maxPlanetNumber = CurrentPlanet + 1;
                Variables.maxStageNumber = 1;
            }

            stageUiManager.UnlockStage(Variables.maxPlanetNumber, Variables.maxStageNumber);
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

        stageUiManager.SetActiveStageMessage(false);
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

    private void AddReward(int currencyType, BigNumber value)
    {
        var itemData = SaveLoadManager.LoadedData.itemSaveData;

        if (itemData.ContainsKey(currencyType))
        {
            itemData[currencyType] += value;
        }
        else
        {
            itemData.Add(currencyType, value);
        }
    }
}
