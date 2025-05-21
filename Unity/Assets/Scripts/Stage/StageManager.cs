#define UseDebug

using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageMonsterManager StageMonsterManager { get; private set; }
    public UnitPartyManager UnitPartyManager { get; private set; }
    public ObjectPoolManager ObjectPoolManager { get; private set; }
    [field: SerializeField]
    public StageUiManager StageUiManager { get; private set; }
    public CameraManager CameraManager { get; private set; }

    [SerializeField]
    public IngameStatus IngameStatus { get; private set; }

    [SerializeField]
    [SerializedDictionary("Status", "Data")]
    private SerializedDictionary<IngameStatus, StageStatusMachineData> statusMachineDatas;

    public LinkedList<IObjectPoolGameObject> backgrounds { get; private set; } = new LinkedList<IObjectPoolGameObject>();

    public LinkedList<DamageText> damageTexts { get; private set; } = new LinkedList<DamageText>();

    private Dictionary<IngameStatus, StageStatusMachine> machines = new Dictionary<IngameStatus, StageStatusMachine>();
    //private StageStatusMachine stageStatusMachine;

    public event System.Action<IngameStatus> OnIngameStatusChanged;
    public event System.Action OnStageEnd;


    private void Awake()
    {
        StageMonsterManager = GetComponent<StageMonsterManager>();
        UnitPartyManager = GetComponent<UnitPartyManager>();
        ObjectPoolManager = GetComponent<ObjectPoolManager>();
        CameraManager = GetComponent<CameraManager>();
        StageUiManager.OnExitButtonClicked += OnExitClicked;
        InitStatusMachines();
    }

    private void Start()
    {
        machines[IngameStatus].Start();
        //stageStatusMachine.Start();
    }

    private void Update()
    {
        //stageStatusMachine.Update();

        machines[IngameStatus].Update();
    }

    private void InitStatusMachines()
    {
        machines.Clear();

        StageStatusMachine stageStatusMachine = new PlanetStageStatusMachine(this);
        stageStatusMachine.SetStageData(statusMachineDatas[IngameStatus.Planet]);
        machines.Add(IngameStatus.Planet, stageStatusMachine);

        stageStatusMachine = new DungeonStageStatusMachine(this);
        stageStatusMachine.SetStageData(statusMachineDatas[IngameStatus.Dungeon]);
        machines.Add(IngameStatus.Dungeon, stageStatusMachine);

        stageStatusMachine = new MineStageStatusMachine(this);
        stageStatusMachine.SetStageData(statusMachineDatas[IngameStatus.Mine]);
        machines.Add(IngameStatus.Mine, stageStatusMachine);

#if UseDebug

        stageStatusMachine = new LevelDesignStageStatusMachine(this);
        machines.Add(IngameStatus.LevelDesign, stageStatusMachine);
#endif
    }

    public void SetStatus(IngameStatus status)
    {
        if (status == IngameStatus)
        {
            return;
        }

        ReleaseDamageTexts();
        StageUiManager.curtain.SetFade(true);
        StageUiManager.UIGroupStatusManager.SetUIStatus(status);

        machines[IngameStatus].SetActive(false);

        OnIngameStatusChanged?.Invoke(status);
        StageUiManager.IngameUIManager.SetStatus(status);

        machines[status].SetActive(true);

        IngameStatus = status;

    }

    public void ResetStage()
    {
        StageUiManager.HPBarManager.ClearHPBar();
        machines[IngameStatus].Reset();
    }

    public void ReleaseDamageTexts()
    {
        while (damageTexts.Count > 0)
        {
            damageTexts.Last.Value.Release();
        }
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
        machines[IngameStatus].Exit();
    }

    public void StageEnd()
    {
        OnStageEnd?.Invoke();
    }

    public StageStatusMachine GetStage(IngameStatus status)
    {
        if (machines.ContainsKey(status))
        {
            return machines[status];
        }
        return null;
    }

    public void MiningBattleStart()
    {
        ((MineStageStatusMachine)machines[IngameStatus.Mine]).StartMineBattle();
    }
}
