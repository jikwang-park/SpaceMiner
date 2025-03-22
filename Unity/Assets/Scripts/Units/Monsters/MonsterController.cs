using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class MonsterController : MonoBehaviour
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

    private Collider[] colliders;
    [SerializeField]
    private LayerMask mask;

    public MonsterLaneManager MonsterLaneManager { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; } = 4f;
    private float aggroRange;
    public float TargetDistance { get; private set; }

    public Transform Target { get; private set; }

    public int currentLane;

    public float LastAttackTime { get; private set; }

    private bool drawRegion;

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
            for(int i = 0; i<colliders.Length;++i)
            {
                colliders[i] = null;
            }

            int frontMonsters = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * 1.5f, new Vector3(0.5f, 0.5f, 1.5f), colliders, Quaternion.identity, mask.value);
            return frontMonsters == 1;
        }
    }

    private void Awake()
    {
        InitBehaviourTree();
        status = Status.Wait;
        colliders = new Collider[3];
    }

    private void Start()
    {
        drawRegion = true;
        MonsterLaneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MonsterLaneManager>();
    }

    private void Update()
    {
        if (Target == null)
        {
            var units = GameObject.FindGameObjectsWithTag("Player");
            float distance = Mathf.Infinity;
            foreach (var unit in units)
            {
                float unitDistance = Mathf.Abs(Vector3.Dot((unit.transform.position - transform.position), Vector3.forward));
                if (unitDistance < distance)
                {
                    Target = unit.transform;
                    distance = unitDistance;
                    TargetDistance = distance;
                }
            }
        }
        else
        {
            TargetDistance = Mathf.Abs(Vector3.Dot((Target.position - transform.position), Vector3.forward));
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
        animations.PlayQueued("Attack_01");
        StartCoroutine(AttackTimer());
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(0.5f);
        status = Status.Wait;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (drawRegion)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position + transform.forward * 1.5f, new Vector3(1f, 1f, 3f));
    }
}
