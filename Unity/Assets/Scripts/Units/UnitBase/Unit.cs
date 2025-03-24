using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;



public class Unit : MonoBehaviour
{
    public StageManager stageManger;

    public CharacterStats unitStats;


    public int unitArmor;



    public GameObject enermyPrefab;
    //탱커 스킬 쿨타임
    public float skillCoolTime = 10f;

    public UnitTypes currentUnitType;
    private BehaviorTree<Unit> behaviorTree;

    public float skillUsingTime = 2.0f;
    public float attackUsingTime = 0.4f;

    public float speed = 20f;
    public int aliveCount = 0;

    public UnitPartyManager unitPartyManager;


    public AttackDefinition unitWeapon;

    private Transform targetPos;

    private BigNumber currentHp;

    private void SetStatus(BigNumber unitMaxHp, BigNumber unitdamage, int unitArmor)
    {
        unitdamage = unitWeapon.damage;

        this.unitStats.maxHp = unitMaxHp;
        this.unitStats.damage = unitdamage;
        this.unitStats.armor = unitArmor;
        this.unitStats.Hp = currentHp;
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
        SetStatus(70, 25, 3);
        currentHp = 40;
    }


    private void SetTankerStats()
    {
        behaviorTree = UnitBTManager.GetBehaviorTree(this, UnitTypes.Tanker);
        SetStatus(100, 15, 10);
        currentHp = 50;
    }
    private void Awake()
    {

        Init();
    }

    private void Start()
    {
        targetPos = stageManger.MonsterLaneManager.GetFirstMonster(0);
    }
    // 유닛 컨디션 bool값
    public bool IsDead // 플레이어가 죽었는지
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

    public bool IsUnitCanAttack // 사정거리 내에 있는지
    {
        get
        {
            if (targetPos.gameObject==null)
            {
                return false;
            }
            if (Vector3.Distance(transform.position, targetPos.position) <= unitWeapon.range)
            {
                return true;
            }
            return false;
        }
    }
    //스킬사용중이니?
    public bool IsSkillUsing;
    //일반 공격중이니?
    public bool IsNormalAttacking;
    //1대 맞았니?
    public bool IsUnitHit;
    //스킬쿨타임 돌았니?
    public bool IsSkillCoolTimeOn
    {
        get
        {
            if(Time.time > lastAttackTime+ skillCoolTime)
                return true;

            return false;
        }
    }
    public bool IsAttackCoolTimeOn
    {
        get
        {
            if(Time.time > lastAttackTime + unitWeapon.coolDown)
                return true;

            return false;
        }
    }
    public float lastAttackTime;
    public float lastSkillAttackTime;
    

    private void Update()
    {
        behaviorTree.Update();

        IsUnitHit = false;
        if(Input.GetKeyDown(KeyCode.M))
        {
            IsUnitHit = true;
        }
    }

    public void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }

    public void AttackCorutine()
    {
        StartCoroutine(NormalAttackCor());
    }

    public IEnumerator NormalAttackCor()
    {
        if (enermyPrefab != null)
        {
            unitWeapon.Execute(gameObject, targetPos.gameObject);
        }
        yield return new WaitForSeconds(attackUsingTime);
        IsNormalAttacking = false;
    }

}
