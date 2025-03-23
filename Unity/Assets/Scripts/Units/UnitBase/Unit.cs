using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;



public class Unit : MonoBehaviour
{




    public BigNumber currentHp;
    public BigNumber unitMaxHp;
    public BigNumber unitDamage;
    public int unitArmor;

    private int maxHp = 20;


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

    private void SetStatus(BigNumber unitMaxHp, BigNumber unitdamage, int unitArmor)
    {
        this.unitMaxHp = unitMaxHp;
        this.unitDamage = unitdamage;
        this.unitArmor = unitArmor;
    }


    private void Init()
    {
        switch (currentUnitType)
        {
            case UnitTypes.Tanker:
                SetTankerStats();
                break;
            case UnitTypes.Dealer:
                behaviorTree = UnitBTManager.GetBehaviorTree(this, UnitTypes.Dealer);
                SetStatus(70, 25, 3);
                break;
        }
    }



    private void SetTankerStats()
    {
        behaviorTree = UnitBTManager.GetBehaviorTree(this, UnitTypes.Tanker);
        SetStatus(100, 15, 10);
    }
    private void Awake()
    {
        var testPos = new Vector3(40, 0, 0);
        Instantiate(enermyPrefab);
        enermyPrefab.transform.position = testPos;

        Init();
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
            if (Vector3.Distance(transform.position,enermyPrefab.transform.position) <= unitWeapon.range)
            {
                return true;
            }
            return false;
        }
    }
    //스킬사용중이니?
    public bool IsSkillUsing;
    //일반 공격중이니?
    public bool IsNormalAttack;
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
        var dir = (enermyPrefab.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    public void AttackCorutine()
    {
        StartCoroutine(NormalAttackCor());
    }

    public IEnumerator NormalAttackCor()
    {
        if (enermyPrefab != null)
        {
            unitWeapon.Execute(gameObject, enermyPrefab.gameObject);
        }
        yield return new WaitForSeconds(unitWeapon.coolDown);
        IsNormalAttack = false;
    }

}
