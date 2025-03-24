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
    //��Ŀ ��ų ��Ÿ��
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
