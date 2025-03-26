using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterController : MonoBehaviour, IObjectPoolGameObject
{
    private readonly int hashAttack = Animator.StringToHash("Attack");

    public enum Status
    {
        Wait,
        Attacking,
    }

    public Status status;

    [field: SerializeField]
    public AttackDefinition weapon { get; private set; }

    [SerializeField]
    private Animation animations;
    private BehaviorTree<MonsterController> behaviorTree;

    public StageManager stageManager { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; } = 4f;

    [field: SerializeField]
    public float minDistanceInMonster { get; private set; } = 2f;
    public float TargetDistance { get; private set; }

    public Transform Target { get; private set; }

    public Func<int, Transform> findFrontMonster;

    public int frontLine = -1;

    public float LastAttackTime { get; private set; }

    private bool isDrawRegion;

    public bool CanAttack
    {
        get
        {
            return weapon.range > TargetDistance && LastAttackTime + weapon.coolDown < Time.time;
        }
    }

    public bool CanMove
    {
        get
        {
            return frontLine < 0 || Vector3.Dot(findFrontMonster(frontLine).position - transform.position, Vector3.back) > minDistanceInMonster;
        }
    }

    public IObjectPool<GameObject> ObjectPool { get; set; }

    private void Awake()
    {
        InitBehaviourTree();
        status = Status.Wait;
    }

    private void OnEnable()
    {
        isDrawRegion = true;

        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnDisable()
    {
        isDrawRegion = false;

        stageManager = null;
    }

    private void Update()
    {
        Target = stageManager.UnitPartyManager.GetFirstLineUnitTransform();

        if (Target is not null)
        {
            TargetDistance = Vector3.Dot(Target.position - transform.position, Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            var destructables = GetComponents<IDestructable>();

            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(null);
            }
        }

        behaviorTree.Update();
    }

    private void InitBehaviourTree()
    {
        behaviorTree = new BehaviorTree<MonsterController>(this);

        var rootSelector = new SelectorNode<MonsterController>(this);

        var attackSequence = new SquenceNode<MonsterController>(this);
        attackSequence.AddChild(new CanAttackMonsterCondition(this));
        attackSequence.AddChild(new AttackMonsterAction(this));

        var rushSequence = new SquenceNode<MonsterController>(this);
        rushSequence.AddChild(new IsUnitExistCondition(this));
        rushSequence.AddChild(new CanMonsterMoveCondition(this));
        rushSequence.AddChild(new RushAction(this));

        rootSelector.AddChild(attackSequence);
        rootSelector.AddChild(rushSequence);

        behaviorTree.SetRoot(rootSelector);
    }

    public void AttackTarget()
    {
        status = Status.Attacking;
        LastAttackTime = Time.time;
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.25f);
        if (Target != null)
            weapon.Execute(gameObject, Target.gameObject);
        yield return new WaitForSeconds(0.25f);
        status = Status.Wait;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isDrawRegion)
        {
            Gizmos.DrawWireCube(transform.position + transform.forward * 0.5f, new Vector3(1f, 1f, 1f));
        }
    }

    public void Release()
    {
        
        ObjectPool.Release(gameObject);
    }
}
