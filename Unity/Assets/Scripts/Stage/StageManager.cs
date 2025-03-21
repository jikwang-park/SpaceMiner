using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageManager : MonoBehaviour
{
    private const string stageIDFormat = "{0:D2}Planet-{1}";

    [field: SerializeField]
    public int CurrentStage { get; private set; } = 1;

    [field: SerializeField]
    public int CurrentSubStage { get; private set; } = 1;
    [field: SerializeField]
    public int CurrentWave { get; private set; } = 0;


    [SerializeField]
    private AssetReferenceGameObject stage;

    private WaveSpawner waveSpawner;

    private void Awake()
    {
        waveSpawner = GetComponent<WaveSpawner>();
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
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            var stageData = DataTableManager.StageTable.GetData(string.Format(stageIDFormat, CurrentStage, CurrentSubStage));
            var waveData = DataTableManager.WaveTable.GetData(stageData.CorpsID);

            var corpsData = DataTableManager.CorpsTable.GetData(waveData.WaveCorpsIDs[CurrentWave]);
            ++CurrentWave;
            waveSpawner.Spawn(transform.position, corpsData);
        }
    }

}
