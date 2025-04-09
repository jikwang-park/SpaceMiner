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

    private void Awake()
    {
        InitBehaviourTree();
    }

    private void Start()
    {
        currentTarget = ore;
        Init(1, 3001, 1);
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
        this.storage = storage;
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
        Slot = slot;
        PlanetData = DataTableManager.PlanetTable.GetData(planetID);
        RobotData = DataTableManager.RobotTable.GetData(robotID);
    }

    public void Release()
    {
        Slot = 0;
        PlanetData = null;
        RobotData = null;
        behaviorTree.Reset();
        ObjectPool.Release(gameObject);
    }
}
