using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageManager : MonoBehaviour
{
    private const string stageIDFormat = "{0:D2}Planet-{1}";

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
    private AssetReferenceGameObject stage;
    [SerializeField]
    private float spawnDistance = 10f;

    private WaveSpawner waveSpawner;

    private HashSet<MonsterController> monsters;

    private StageTable.Data stageData;
    private WaveTable.Data waveData;

    private float stageStartTime;

    private WaitForSeconds wait1 = new WaitForSeconds(1f);


    private void Awake()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        MonsterLaneManager = GetComponent<MonsterLaneManager>();
        UnitPartyManager = GetComponent<UnitPartyManager>();
        monsters = new HashSet<MonsterController>();
            
        SaveLoadManager.LoadGame();
        DoLoad();
    }

    private void Start()
    {
        SetStageInfo();
        SaveLoadManager.onSaveRequested += DoSave;
        Addressables.InstantiateAsync(stage, Vector3.back * 10f, Quaternion.identity);
        SpawnNextWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentPlanet, CurrentStage));
            Debug.Log(stageData.CorpsID);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var handle = Addressables.InstantiateAsync(stage);
            handle.WaitForCompletion();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnNextWave();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (int i = 0; i < 3; ++i)
            {
                Debug.Log(this.MonsterLaneManager.GetFirstMonster(i));
            }
        }

        float remainTime = 60f + stageStartTime - Time.time;

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
        stageStartTime = Time.time;

        stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentPlanet, CurrentStage));
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

        yield return wait1;
        Addressables.LoadSceneAsync("StageDevelopScene");
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

        yield return wait1;

        if (Variables.stageMode == StageMode.Ascend)
        {
            if (DataTableManager.StageTable.IsExistStage(CurrentPlanet, CurrentStage + 1))
            {
                ++Variables.stageNumber;

                if (Variables.maxPlanetNumber == CurrentPlanet
                    && Variables.maxStageNumber < CurrentStage + 1)
                {
                    ++Variables.maxStageNumber;

                    stageUiManager.UnlockStage(CurrentPlanet, CurrentStage + 1);
                }
            }
            else if (DataTableManager.StageTable.IsExistPlanet(CurrentPlanet + 1))
            {
                ++Variables.planetNumber;
                Variables.stageNumber = 1;

                if (Variables.maxPlanetNumber < Variables.planetNumber)
                {
                    Variables.maxPlanetNumber = Variables.planetNumber;
                    Variables.maxStageNumber = Variables.stageNumber;
                    stageUiManager.UnlockStage(Variables.planetNumber, Variables.stageNumber);
                }
            }
        }

        SetStageInfo();
        SpawnNextWave();

        stageUiManager.SetActiveStageMessage(false);
    }

    private void DoSave(TotalSaveData totalSaveData)
    {
        StageSaveData stageSaveData = new StageSaveData
        {
            currentPlanet = CurrentPlanet,
            currentStage = CurrentStage,
            HighPlanet = CurrentPlanet,
            HighStage = CurrentStage,
        };
        totalSaveData.stageSaveData = stageSaveData;
    }
    private void DoLoad()
    {
        StageSaveData stageLoadData = SaveLoadManager.LoadedData.stageSaveData;

        if(stageLoadData != null)
        {
            Variables.planetNumber = stageLoadData.currentPlanet;
            Variables.stageNumber = stageLoadData.currentStage;
            // �ְ��� �� �༺ ������ �ҷ�����
            // �ְ��� �� �������� ������ �ҷ�����
        }
    }
}
