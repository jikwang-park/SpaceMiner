using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;



public class Unit : MonoBehaviour
{
    //Base Stats\
    public UnitTypes UnitTypes
    {
        get
        {
            return currentUnitType;
        }

    }
    private UnitTypes currentUnitType;


    public BigNumber barrier;

    public bool HasBarrier
    {
        get
        {
            if (barrier > 0)
                return true;

            return false;
        }
    }

    public StageManager stageManger;

    public UnitStats unitStats;





    public float targetDistance;

    public float skillCoolTime = 10f;

    public BehaviorTree<Unit> behaviorTree;

    public float skillUsingTime = 2.0f;
    public float attackUsingTime = 0.4f;


    public int aliveCount = 0;

    [SerializeField]
    private SoldierTable.Data soldierData;
    [SerializeField]
    public UnitSkill unitSkill;

    public Transform targetPos;

    private bool isTargetInArea = false;

    private int lane = 0;

    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        stageManger = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    // ���� ����� bool��
    public bool IsDead // �÷��̾ �׾�����
    {
        get
        {
            if (unitStats.Hp <= 0)
            {
                return true;
            }
            return false;
        }
    }

    public float lastSkillUsingTime;

    public bool CanUseSkill
    {
        get
        {
            if (Time.time < unitSkill.coolTime + lastSkillUsingTime)
            {
                return false;
            }
            return true;
        }
    }


    public bool IsUnitCanAttack // �����Ÿ� ���� �ִ���
    {
        get
        {
            if (targetPos == null)
                return false;

            if (targetDistance <= unitStats.range && IsAttackCoolTimeOn)
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
            if (Time.time > lastAttackTime + unitStats.coolDown)
                return true;

            return false;
        }
    }
    public float lastAttackTime;
    public float lastSkillAttackTime;

    private bool isMonsterSpawn;


    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        unitStats.SetData(data, type);

        currentUnitType = type;
        behaviorTree = UnitBTManager.SetBehaviorTree(this, currentUnitType);
    }




    public bool IsMonsterExist()
    {
        var lane = stageManger.MonsterLaneManager.LaneCount;

        for (int i = 0; i < lane; ++i)
        {
            var target = stageManger.MonsterLaneManager.GetMonsterCount(i);

            if (target > 0)
            {
                return true;

            }
        }
        return false;
    }


    private Transform GetTargetPosition()
    {
        var lane = stageManger.MonsterLaneManager.LaneCount;

        for (int i = 0; i < lane; ++i)
        {
            var target = stageManger.MonsterLaneManager.GetMonsterCount(i);
            var targetPosition = stageManger.MonsterLaneManager.GetFirstMonster(i);

            if (target > 0)
            {

                targetDistance = Vector3.Dot(targetPosition.position - stageManger.UnitPartyManager.generateInstance[0].transform.position, Vector3.forward);
                if (targetDistance <= unitStats.range)
                {
                    targetPos = targetPosition;
                    return targetPos;
                }
            }
        }
        return null;
    }

    public void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime * unitStats.moveSpeed;
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
            unitStats.Execute(targetPos.gameObject);
        }
        yield return new WaitForSeconds(attackUsingTime);
        IsNormalAttacking = false;
    }
    public void UseSkill()
    {
        if (Time.time < unitSkill.coolTime + lastSkillUsingTime)
        {
            return;
        }
        unitSkill.ExecuteSkill();
        lastSkillUsingTime = Time.time;
    }

    private float skillEndTime;

    public void SetBarrier(float time, BigNumber amount)
    {
        skillEndTime = Time.time + time;
        barrier = amount;

    }


   

    private void Update()
    {
        if (HasBarrier)
        {
            if (Time.time > skillEndTime)
            {
                barrier = 0;
            }
        }



        GetTargetPosition();
        behaviorTree.Update();

        IsUnitHit = false;
        if (Input.GetKeyDown(KeyCode.M))
        {
            IsUnitHit = true;
        }

    }
}
