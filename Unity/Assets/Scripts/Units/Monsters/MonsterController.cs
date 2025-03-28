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
        SkillUsing,
    }

    public Status status;

    [field: SerializeField]
    public MonsterStats Stats { get; private set; }

    [SerializeField]
    private Animation animations;
    private BehaviorTree<MonsterController> behaviorTree;

    public StageManager StageManager { get; private set; }

    [field: SerializeField]
    public float minDistanceInMonster { get; private set; } = 2f;

    public float TargetDistance { get; private set; }
    public Transform Target { get; private set; }
    public MonsterTable.Data MonsterData { get; private set; }


    public Func<int, Transform> findFrontMonster;

    public int frontLine = -1;

    public float LastAttackTime { get; private set; }

    private bool isDrawRegion;

    public bool CanAttack
    {
        get
        {
            return Stats.range > TargetDistance && LastAttackTime + Stats.coolDown < Time.time;
        }
    }

    public bool CanMove
    {
        get
        {
            return frontLine < 0 || -(findFrontMonster(frontLine).position.z - transform.position.z) > minDistanceInMonster;
        }
    }

    public IObjectPool<GameObject> ObjectPool { get; set; }

    private void Awake()
    {
        Stats = GetComponent<MonsterStats>();
        status = Status.Wait;
    }

    private void OnEnable()
    {
        isDrawRegion = true;

        StageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnDisable()
    {
        isDrawRegion = false;
        StageManager = null;
    }

    private void Update()
    {
        Target = StageManager.UnitPartyManager.GetFirstLineUnitTransform();

        if (Target is not null)
        {
            TargetDistance = -(Target.position.z - transform.position.z);
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

        if (MonsterData.MonsterSkill != "0")
        {
            var skillSequence = new SquenceNode<MonsterController>(this);
            skillSequence.AddChild(new IsMonsterSkillCooltimeCondition(this));
            skillSequence.AddChild(new IsMonsterSkillTargetExistCondition(this));
            skillSequence.AddChild(new SkillMonsterAction(this));
            rootSelector.AddChild(skillSequence);
        }

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

    public void SetMonsterId(string monsterId)
    {
        MonsterData = DataTableManager.MonsterTable.GetData(monsterId);
        Stats.SetData(MonsterData);
        InitBehaviourTree();
        if (MonsterData.MonsterSkill != "0")
        {
            GetComponent<MonsterSkill>().SetSkill(MonsterData.MonsterSkill);
        }
    }

    public void AttackTarget()
    {
        status = Status.Attacking;
        LastAttackTime = Time.time;
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer()
    {
        //TODO: 애니메이션 정의되거나 공격 정의 후 수정 필요
        yield return new WaitForSeconds(0.25f);
        if (Target != null)
            Stats.Execute(Target.gameObject);
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
