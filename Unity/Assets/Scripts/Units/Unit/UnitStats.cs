using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnitUpgradeTable;


public class UnitStats : CharacterStats
{
    private StageManager stageManager;

    private Grade currentGrade;
    private float criticalPercent;

    private BigNumber FinialDamage
    {
        get
        {
            return ((baseDamage + accountDamage) * (1f + buildingAttackDamage));
        }
    }

    private BigNumber defalutValue = 1;
    //�⺻
    private BigNumber baseDamage;
    private BigNumber accountDamage = 0;
    private float buildingAttackDamage = 0;
    //ũ��
    private float accountCriticalDamage = 0f;
    private float buildingCriticalDamage = 0f;
    //���
    private BigNumber baseArmor;
    private BigNumber accountArmor = 0;
    private float buildingArmor = 0;
    //ü��
    private BigNumber baseMaxHp;
    private BigNumber accountHp = 0;
    private float buildingHp = 0;
    //ũȮ
    private float accountCriticalChance = 0f;
    private float buildingCriticalChance = 0f;


    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void Start()
    {
    }
    public void AddStats(UpgradeType type, float amount)
    {
        switch (type)
        {
            case UpgradeType.AttackPoint:
                accountDamage += (int)amount;
                break;
            case UpgradeType.HealthPoint:
                accountHp += (int)amount;
                RecalculateHpWithIncrease();
                break;
            case UpgradeType.DefensePoint:
                accountArmor += (int)amount;
                RecalculateArmor();
                break;
            case UpgradeType.CriticalPossibility:
                 accountCriticalChance += (int)amount;
                break;
            case UpgradeType.CriticalDamages:
                accountCriticalDamage += (int)amount;
                break;
        }
    }

    private void RecalculateArmor()
    {
        armor = (baseArmor + accountArmor) * (1 + buildingArmor);
    }

    private void RecalculateHpWithIncrease()
    {
        BigNumber previousMaxHp = maxHp;
        maxHp = (baseMaxHp + accountHp) * (1f + buildingHp);

        BigNumber increase = maxHp - previousMaxHp;

        Hp += increase;
        if (Hp > maxHp)
            Hp = maxHp;
    }

    private void RecalculateHpWithRatio()
    {
        float ratio = 0f;
        if(maxHp>0)
        {
            ratio = Hp.DivideToFloat(maxHp);
        }
        else
        {
            ratio = 1f;
        }
        maxHp = (baseMaxHp + accountHp) * (1f + buildingHp);
        Hp = maxHp * ratio;
    }

    public void AddBuildingStats(BuildingTable.BuildingType type, float amount)
    {
        

        switch (type)
        {
            case BuildingTable.BuildingType.IdleTime:
                break;
            case BuildingTable.BuildingType.AttackPoint:
                buildingAttackDamage += (int)amount;
                break;
            case BuildingTable.BuildingType.HealthPoint:
                buildingHp += (int)amount;
                RecalculateHpWithIncrease();
                break;
            case BuildingTable.BuildingType.DefensePoint:
                buildingArmor += amount;
                RecalculateArmor();
                break;
            case BuildingTable.BuildingType.CriticalPossibility:
                buildingCriticalChance += amount;
                break;
            case BuildingTable.BuildingType.CriticalDamages:
                buildingCriticalDamage += amount;
                break;
            case BuildingTable.BuildingType.Gold:
                break;
            case BuildingTable.BuildingType.Mining:
                break;
        }
    }



    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = data.MoveSpeed;
        baseDamage = data.Attack;

        baseMaxHp = data.HP;
        currentGrade = data.Grade;

        coolDown = 1;
        baseArmor = data.Defence;
        range = data.Range;
        RecalculateArmor();

        RecalculateHpWithRatio();
    }


    public override void Execute(GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }
    public override Attack CreateAttack(CharacterStats defenderStats)
    {
        Attack attack = new Attack();


        criticalChance = accountCriticalChance + buildingCriticalChance;

        attack.isCritical = criticalChance >= Random.Range(0, 100);
        if (attack.isCritical)
        {
            criticalPercent = (2 + (accountCriticalDamage + buildingCriticalDamage));

            attack.damage = FinialDamage * criticalPercent;
        }
        else
        {
            attack.damage = FinialDamage;
        }


        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }
    public void SkillExecute(GameObject defender) // ��ų ������ ����� ���� �Ѱܼ� ������ ó��
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateDealerSkillAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }

    public Attack CreateDealerSkillAttack(CharacterStats defenderStats)
    {
        Attack attack = new Attack();

        var data = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        var id = data[UnitTypes.Dealer][currentGrade];

        var currentData = DataTableManager.DealerSkillTable.GetData(id);

        BigNumber finialSkillDamage = FinialDamage * currentData.DamageRatio;


        criticalChance = accountCriticalChance + buildingCriticalChance;

        attack.isCritical = criticalChance >= Random.Range(0, 100);
        if (attack.isCritical)
        {
            criticalPercent = (2 + (accountCriticalDamage + buildingCriticalDamage));
            attack.damage = finialSkillDamage * criticalPercent;
        }
        else
        {
            attack.damage = finialSkillDamage;
        }


        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }


}
