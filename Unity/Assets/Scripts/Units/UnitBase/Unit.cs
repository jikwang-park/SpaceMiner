using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;



public class Unit : MonoBehaviour
{
    public StageManager stageManger;

    public CharacterStats unitStats;



    private BigNumber unitMaxHp;
    private BigNumber unitDamage;
    private int unitArmor;
    private BigNumber currentHp;


    private float targetDistance;

    //��Ŀ ��ų ��Ÿ��
    public float skillCoolTime = 10f;

    public UnitTypes currentUnitType;
    private BehaviorTree<Unit> behaviorTree;

    public float skillUsingTime = 2.0f;
    public float attackUsingTime = 0.4f;

    public float speed = 20f;
    public int aliveCount = 0;



    public UnitWeapon unitWeapon;

    private Transform targetPos;

    private bool isTargetInArea = false;

    private int lane = 0;

    private void SetStatus(BigNumber unitMaxHp, int unitArmor)
    {
        unitStats.maxHp = unitMaxHp;
        unitStats.Hp = currentHp;
        unitStats.armor = unitArmor;

    }


    private void Init()
    {
        switch (currentUnitType)
        {
            case UnitTypes.Tanker:
                SetTankerStats();
                break;
            case UnitTypes.Dealer:
                SetDealerStats();
                break;
        }
    }
    private void SetDealerStats()
    {
        behaviorTree = UnitBTManager.GetBehaviorTree(this, UnitTypes.Dealer);
        SetStatus(70, 3);
        currentHp = 40;
    }


    private void SetTankerStats()
    {
        behaviorTree = UnitBTManager.GetBehaviorTree(this, UnitTypes.Tanker);
        SetStatus(100, 10);
        currentHp = 50;
    }
    private void Awake()
    {
        stageManger = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        unitStats = GetComponent<CharacterStats>();
        Init();
    }



    private void Start()
    {
    }
    // ���� ����� bool��
    public bool IsDead // �÷��̾ �׾�����
    {
        get
        {
            if (currentHp <= 0)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsUnitCanAttack // �����Ÿ� ���� �ִ���
    {
        get
        {
            if (targetPos == null)
                return false;

            if (targetDistance <= unitWeapon.range)
            {
                return true;
            }
            return false;
        }
    }
    //��ų������̴�?
    public bool IsSkillUsing;
    //�Ϲ� �������̴�?
    public bool IsNormalAttacking;
    //1�� �¾Ҵ�?
    public bool IsUnitHit;
    //��ų��Ÿ�� ���Ҵ�?
    public bool IsSkillCoolTimeOn
    {
        get
        {
            if (Time.time > lastAttackTime + skillCoolTime)
                return true;

            return false;
        }
    }
    public bool IsAttackCoolTimeOn
    {
        get
        {
            if (Time.time > lastAttackTime + unitWeapon.coolDown)
                return true;

            return false;
        }
    }
    public float lastAttackTime;
    public float lastSkillAttackTime;

    private bool isMonsterSpawn;


    private void Update()
    {
        GetTargetPosition();
        behaviorTree.Update();

        IsUnitHit = false;
        if (Input.GetKeyDown(KeyCode.M))
        {
            IsUnitHit = true;
        }
       
    }

    private Transform GetTargetPosition()
    {
        var lane = stageManger.MonsterLaneManager.LaneCount;

        for(int i = 0; i< lane; ++i)
        {
            var target = stageManger.MonsterLaneManager.GetMonsterCount(i);
            var targetPosition = stageManger.MonsterLaneManager.GetFirstMonster(i);

            if (target > 0)
            {

                targetDistance = Vector3.Distance(stageManger.UnitPartyManager.generateInstance[0].transform.position, targetPosition.position);
                if(targetDistance <= unitWeapon.range)
                {
                    targetPos = targetPosition;
                    Debug.Log(targetPos.position);
                    return targetPos;
                }
            }
        }
        return null;
    }

    public void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }

    public void AttackCorutine()
    {
        StartCoroutine(NormalAttackCor());
        lastAttackTime = Time.time;
    }

    public IEnumerator NormalAttackCor()
    {
        if (targetPos.gameObject != null)
        {
            unitWeapon.Execute(gameObject, targetPos.gameObject);
        }
        yield return new WaitForSeconds(attackUsingTime);
        IsNormalAttacking = false;
    }
}
