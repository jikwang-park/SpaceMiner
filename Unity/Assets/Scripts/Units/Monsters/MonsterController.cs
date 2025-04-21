using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterController : MonoBehaviour, IObjectPoolGameObject
{
    public enum Status
    {
        Wait,
        Attacking,
        SkillUsing,
        Dead,
        Run,
    }

    public Status status;

    [field: SerializeField]
    public MonsterStats Stats { get; private set; }

    [SerializeField]
    private float attackTime = 0.4f;

    private BehaviorTree<MonsterController> behaviorTree;

    public AnimationControl AnimationController { get; private set; }

    public StageManager StageManager { get; private set; }

    [field: SerializeField]
    public float minDistanceInMonster { get; private set; } = 2f;

    public float TargetDistance { get; private set; }
    public Transform Target { get; private set; }

    public bool hasTarget = false;
    public MonsterTable.Data MonsterData { get; private set; }
    public MonsterRewardTable.Data RewardData { get; private set; }


    public Func<int, Transform> findFrontMonster;
    public Func<int, bool> isFrontMonster;

    public int currentLine = -1;

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
            return isFrontMonster(currentLine) || -(findFrontMonster(currentLine).position.z - transform.position.z) > minDistanceInMonster;
        }
    }

    public IObjectPool<GameObject> ObjectPool { get; set; }


    private void Awake()
    {
        Stats = GetComponent<MonsterStats>();
        AnimationController = GetComponent<AnimationControl>();
        status = Status.Wait;
    }

    private void Start()
    {
        AnimationController.AddEvent(AnimationControl.AnimationClipID.Attack, attackTime, OnAttack);
        AnimationController.AddEvent(AnimationControl.AnimationClipID.Attack, 1f, OnAttackEnd);
    }

    private void OnEnable()
    {
        isDrawRegion = true;
        hasTarget = false;
        currentLine = -1;
        TargetDistance = float.PositiveInfinity;
        GetComponent<DestructedDestroyEvent>().OnDestroyed += OnThisDie;
        StageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        status = Status.Wait;
    }

    private void OnDisable()
    {
        isDrawRegion = false;
    }

    private void Update()
    {
        if (status == Status.Dead)
        {
            return;
        }

        if (StageManager.UnitPartyManager.UnitCount > 0)
        {
            var newTarget = StageManager.UnitPartyManager.GetFirstLineUnitTransform();
            if (newTarget != Target)
            {
                if (hasTarget && Target is not null)
                {
                    Target.GetComponent<DestructedDestroyEvent>().OnDestroyed -= OnTargetDie;
                }
                hasTarget = true;
                newTarget.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnTargetDie;
                Target = newTarget;
            }
            TargetDistance = -(Target.position.z - transform.position.z);
        }
        else
        {
            hasTarget = false;
            if (Target is not null)
            {
                Target.GetComponent<DestructedDestroyEvent>().OnDestroyed -= OnTargetDie;
            }
            Target = null;
            TargetDistance = float.PositiveInfinity;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            var destructables = GetComponents<IDestructable>();

            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(null);
            }
        }
#endif

        behaviorTree.Update();
    }

    private void InitBehaviourTree()
    {
        behaviorTree = new BehaviorTree<MonsterController>(this);

        var rootSelector = new SelectorNode<MonsterController>(this);

        if (MonsterData.MonsterSkillID != 0)
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
        rootSelector.AddChild(new MonsterIdleAction(this));

        behaviorTree.SetRoot(rootSelector);
    }

    public void SetMonsterId(int monsterId)
    {
        MonsterData = DataTableManager.MonsterTable.GetData(monsterId);
        Stats.SetData(MonsterData);
        RewardData = DataTableManager.MonsterRewardTable.GetData(MonsterData.RewardTableID);
        InitBehaviourTree();
        if (MonsterData.MonsterSkillID != 0)
        {
            GetComponent<MonsterSkill>().SetSkill(MonsterData.MonsterSkillID, Stats);
        }
    }

    public void AttackTarget()
    {
        status = Status.Attacking;
        LastAttackTime = Time.time;
        AnimationController.Play(AnimationControl.AnimationClipID.Attack);
    }

    private void OnAttack()
    {
        if (Target is not null)
        {
            Stats.Execute(Target.gameObject);
        }
    }

    private void OnAttackEnd()
    {
        AnimationController.Play(AnimationControl.AnimationClipID.BattleIdle);
        status = Status.Wait;
    }

    private void OnThisDie(DestructedDestroyEvent sender)
    {
        status = Status.Dead;
    }

    private void OnTargetDie(DestructedDestroyEvent sender)
    {
        if (Target is not null)
        {
            Target.GetComponent<DestructedDestroyEvent>().OnDestroyed -= OnTargetDie;
        }
        Target = null;
        hasTarget = false;
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
        AnimationController.Stop();
        StageManager.StageMonsterManager.RemoveFromMonsterSet(this);
        ObjectPool.Release(gameObject);
    }
}
