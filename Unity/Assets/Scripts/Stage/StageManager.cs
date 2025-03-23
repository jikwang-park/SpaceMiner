using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageManager : MonoBehaviour
{
    private const string stageIDFormat = "{0:D2}Planet-{1}";
    private const string stageTextFormat = "{0}-{1}\n{2} Wave";


    [field: SerializeField]
    public int CurrentStage { get; private set; }

    [field: SerializeField]
    public int CurrentSubStage { get; private set; }
    [field: SerializeField]
    public int CurrentWave { get; private set; }

    [field: SerializeField]
    public MonsterLaneManager MonsterLaneManager { get; private set; }

    [SerializeField]
    private TextMeshProUGUI stageText;


    [SerializeField]
    private AssetReferenceGameObject stage;

    private WaveSpawner waveSpawner;


    private HashSet<MonsterController> monsters;

    private StageTable.Data stageData;
    private WaveTable.Data waveData;

    private void Awake()
    {
        waveSpawner = GetComponent<WaveSpawner>();
        this.MonsterLaneManager = GetComponent<MonsterLaneManager>();
        monsters = new HashSet<MonsterController>();
        CurrentStage = Variables.stageNumber;
        CurrentSubStage = Variables.stageSubNumber;
        CurrentWave = 0;

        stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentStage, CurrentSubStage));
        waveData = DataTableManager.WaveTable.GetData(stageData.CorpsID);
        stageText.text = string.Format(stageTextFormat, CurrentStage, CurrentSubStage, CurrentWave);

        Invoke("SpawnNextWave", 2f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentStage, CurrentSubStage));
            Debug.Log(stageData.CorpsID);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentStage, CurrentSubStage));

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
    }

    public void AddMonster(MonsterController monsterController)
    {
        monsters.Add(monsterController);

        var destroyEvent = monsterController.GetComponent<DestructedDestroyEvent>();
        destroyEvent.OnDestroyed += () =>
        {
            monsters.Remove(monsterController);
            if (monsters.Count == 0)
            {
                if (CurrentWave >= waveData.WaveCorpsIDs.Length)
                {
                    Addressables.LoadSceneAsync("StageDevelopScene");
                    return;
                }

                Invoke("SpawnNextWave", 2f);
            }
        };
    }

    public void SpawnNextWave()
    {
        var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[CurrentWave]);
        ++CurrentWave;

        waveSpawner.Spawn(transform.position, corpsData);
        stageText.text = string.Format(stageTextFormat, CurrentStage, CurrentSubStage, CurrentWave);
    }
}
