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
            if (Vector3.Distance(transform.position,enermyPrefab.transform.position) <= unitWeapon.range)
            {
                return true;
            }
            return false;
        }
    }
    //��ų������̴�?
    public bool IsSkillUsing;
    //�Ϲ� �������̴�?
    public bool IsNormalAttack;
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
