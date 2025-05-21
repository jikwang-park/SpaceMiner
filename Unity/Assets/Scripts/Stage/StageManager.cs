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
    public ObjectPoolManager ObjectPoolManager { get; private set; }
    [field: SerializeField]
    public StageUiManager StageUiManager { get; private set; }
    public CameraManager CameraManager { get; private set; }

    [SerializeField]
    private IngameStatus ingameStatus;

    public LinkedList<IObjectPoolGameObject> backgrounds { get; private set; } = new LinkedList<IObjectPoolGameObject>();

    private Dictionary<IngameStatus, StageStatusMachine> machines = new Dictionary<IngameStatus, StageStatusMachine>();
    //private StageStatusMachine stageStatusMachine;

    private void Awake()
    {
        StageMonsterManager = GetComponent<StageMonsterManager>();
        UnitPartyManager = GetComponent<UnitPartyManager>();
        ObjectPoolManager = GetComponent<ObjectPoolManager>();
        CameraManager = GetComponent<CameraManager>();

        Init();

        machines.Add(IngameStatus.Planet, new PlanetStageStatusMachine(this));
        machines.Add(IngameStatus.Dungeon, new DungeonStageStatusMachine(this));

        //switch (ingameStatus)
        //{
        //    case IngameStatus.Planet:
        //        stageStatusMachine = new PlanetStageStatusMachine(this);
        //        break;
        //    case IngameStatus.Dungeon:
        //        stageStatusMachine = new DungeonStageStatusMachine(this);
        //        break;
        //}
    }

    private void Start()
    {
        machines[ingameStatus].Start();
        //stageStatusMachine.Start();
    }

    private void Update()
    {
        //stageStatusMachine.Update();

        machines[ingameStatus].Update();
    }

    public void SetStatus(IngameStatus status)
    {
        if (status == ingameStatus)
        {
            return;
        }

        StageUiManager.curtain.SetFade(true);
        StageUiManager.UIGroupStatusManager.SetUIStatus(status);

        machines[ingameStatus].SetActive(false);

        StageUiManager.IngameUIManager.SetStatus(status);

        machines[status].SetActive(true);

        StageUiManager.curtain.SetFade(false);

        ingameStatus = status;

        //switch (status)
        //{
        //    case IngameStatus.Planet:
        //        SceneManager.LoadScene(0);
        //        break;
        //    case IngameStatus.Dungeon:
        //        Addressables.LoadSceneAsync("Scenes/DungeonScene").WaitForCompletion();
        //        break;
        //}
    }

    public void Init()
    {
        var saveData = SaveLoadManager.Data.stageSaveData;

        List<int> dungeons = DataTableManager.DungeonTable.DungeonTypes;

        bool changed = false;

        foreach (var type in dungeons)
        {
            if (!saveData.highestDungeon.ContainsKey(type))
            {
                changed = true;
                saveData.highestDungeon.Add(type, 1);
            }
        }

        if (changed)
        {
            SaveLoadManager.SaveGame();
        }
    }

    public void ResetStage()
    {
        machines[ingameStatus].Reset();
    }

    public void ReleaseBackground()
    {
        while (backgrounds.Count > 0)
        {
            backgrounds.First.Value.Release();
        }
    }

    //TODO: �ν����Ϳ��� ������ ��ư�� ����
    public void OnExitClicked()
    {
        machines[ingameStatus].Exit();
    }
}
