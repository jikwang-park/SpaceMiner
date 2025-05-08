using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MiningRobotController : MonoBehaviour, IObjectPoolGameObject
{
    public PlanetTable.Data PlanetData { get; private set; }
    public RobotTable.Data RobotData { get; private set; }
    public int Slot { get; private set; }

    public float sqrDistance { get; private set; }
    public IObjectPool<GameObject> ObjectPool { get; set; }

    [SerializeField]
    private Transform ore;
    [SerializeField]
    private Transform storage;

    private BehaviorTree<MiningRobotController> behaviorTree;

    [HideInInspector]
    public Transform currentTarget;

    private AnimationControl animationControl;

    public int MiningSpeed { get; private set; }
    public int ProductCapacity { get; private set; }

    public float MoveSpeed { get; private set; }

    private void Awake()
    {
        InitBehaviourTree();
        animationControl = GetComponent<AnimationControl>();
    }

    private void Start()
    {
        EffectItemInventoryManager.OnEffectItemLevelUp += OnEffectItemLevelUp;
    }


    private void Update()
    {
        UpdateDistance();
        behaviorTree.Update();
    }

    public void UpdateDistance()
    {
        sqrDistance = Vector3.SqrMagnitude(transform.position - currentTarget.position);
    }

    public void ChangeTarget(bool isOre)
    {
        if (isOre)
        {
            currentTarget = ore;
        }
        else
        {
            currentTarget = storage;
        }
        UpdateDistance();
    }

    public void SetOreStorage(Transform ore, Transform storage)
    {
        this.ore = ore;
        currentTarget = this.ore;
        this.storage = storage;

        SetMoveSpeed();
    }

    private void InitBehaviourTree()
    {
        behaviorTree = new BehaviorTree<MiningRobotController>(this);

        var rootSequence = new SquenceNode<MiningRobotController>(this);

        rootSequence.AddChild(new RobotMoveAction(this));
        rootSequence.AddChild(new MiningAction(this));
        rootSequence.AddChild(new RobotMoveAction(this));
        rootSequence.AddChild(new ExtractAction(this));

        behaviorTree.SetRoot(rootSequence);
    }

    public void Init(int planetID, int robotID, int slot)
    {
        ResetBehaviorTree();
        Slot = slot;
        PlanetData = DataTableManager.PlanetTable.GetData(planetID);
        RobotData = DataTableManager.RobotTable.GetData(robotID);

        SetRobotStat();
    }

    public void ResetBehaviorTree()
    {
        behaviorTree.Reset();
    }

    public void Release()
    {
        Slot = 0;
        PlanetData = null;
        RobotData = null;
        animationControl.Stop();
        ObjectPool.Release(gameObject);
    }

    private void SetMoveSpeed()
    {
        float distance = Vector3.Distance(ore.position, storage.position);
        MoveSpeed = RobotData.MoveSpeed * distance;
        switch (Slot)
        {
            case 0:
                MoveSpeed /= PlanetData.Distance1;
                break;
            case 1:
                MoveSpeed /= PlanetData.Distance2;
                break;
        }
    }

    private void SetRobotStat()
    {
        int productCapacityLevel = EffectItemInventoryManager.GetLevel(EffectItemTable.ItemType.MiningRobotCapacity);
        int productCapacityEffect = (int)DataTableManager.EffectItemTable.GetDatas(EffectItemTable.ItemType.MiningRobotCapacity)[productCapacityLevel].Value;
        int miningSpeedLevel = EffectItemInventoryManager.GetLevel(EffectItemTable.ItemType.MiningRobotProductSpeed);
        int miningSpeedEffect = (int)DataTableManager.EffectItemTable.GetDatas(EffectItemTable.ItemType.MiningRobotProductSpeed)[miningSpeedLevel].Value;

        ProductCapacity = RobotData.ProductCapacity + productCapacityEffect;
        MiningSpeed = RobotData.MiningSpeed + miningSpeedEffect;
    }

    private void OnEffectItemLevelUp(int itemID)
    {
        var type = DataTableManager.EffectItemTable.GetTypeByID(itemID);
        if (type == EffectItemTable.ItemType.MiningRobotProductSpeed)
        {
            SetMoveSpeed();
        }
        if (type == EffectItemTable.ItemType.MiningRobotCapacity
            || type == EffectItemTable.ItemType.MiningRobotMoveSpeed)
        {
            SetRobotStat();
        }
    }
}
