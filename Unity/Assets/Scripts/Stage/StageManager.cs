using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public StageMonsterManager StageMonsterManager { get; private set; }
    public UnitPartyManager UnitPartyManager { get; private set; }
    public ObjectPoolManager objectPoolManager { get; private set; }
    [field: SerializeField]
    public StageUiManager StageUiManager { get; private set; }

    [SerializeField]
    private IngameStatus ingameStatus;

    private StageStatusMachine stageStatusMachine;

    private void Awake()
    {
        StageMonsterManager = GetComponent<StageMonsterManager>();
        UnitPartyManager = GetComponent<UnitPartyManager>();
        objectPoolManager = GetComponent<ObjectPoolManager>();

        switch(ingameStatus)
        {
            case IngameStatus.Planet:
                stageStatusMachine = new PlanetStageStatusMachine(this);
                break;
            case IngameStatus.Dungeon:
                stageStatusMachine = new DungeonStageStatusMachine(this);
                break;
        }
        SaveLoadManager.LoadGame();
        DoLoad();
    }

    private void Start()
    {
        SaveLoadManager.onSaveRequested += DoSave;
        stageStatusMachine.Start();
    }

    private void Update()
    {
        stageStatusMachine.Update();
    }

    public void SetStatus(IngameStatus status)
    {
        if (status == ingameStatus)
        {
            return;
        }

        switch (status)
        {
            case IngameStatus.Planet:
                SceneManager.LoadScene(0);
                break;
            case IngameStatus.Dungeon:
                Addressables.LoadSceneAsync("Scenes/DungeonScene").WaitForCompletion();
                break;
        }
    }

    //TODO: �ν����Ϳ��� ������ ��ư�� ����
    public void OnExitClicked()
    {
        stageStatusMachine.Exit();
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
            Debug.Log("����� �����Ͱ� �����ϴ�. �⺻ ������ �����մϴ�.");
            return;
        }

        StageSaveData stageLoadData = SaveLoadManager.LoadedData.stageSaveData;

        if (stageLoadData != null)
        {
            Variables.planetNumber = stageLoadData.currentPlanet;
            Variables.stageNumber = stageLoadData.currentStage;
            Variables.maxPlanetNumber = stageLoadData.highPlanet;
            Variables.maxStageNumber = stageLoadData.highStage;
            // �ְ��� �� �༺ ������ �ҷ�����
            // �ְ��� �� �������� ������ �ҷ�����
        }
    }
}
