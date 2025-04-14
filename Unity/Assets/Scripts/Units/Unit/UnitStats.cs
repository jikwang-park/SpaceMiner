using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnitUpgradeTable;

public class UnitStats : CharacterStats
{
    private StageManager stageManager;
    private DealerSkill dealerSkill;

    private Grade currentGrade;

    private BigNumber FinialDamage
    {
        get
        {
            return (defalutValue * ((1f + buildingAttackDamage)) * (baseDamage + accountDamage));
        }
    }

    private BigNumber defalutValue = 1;
    //�⺻
    private BigNumber baseDamage;
    private BigNumber accountDamage = 0;
    private BigNumber buildingAttackDamage = 1;
    //ũ��
    private float accountCriticalDamage = 0f;
    private float buildingCriticalDamage = 0f;
    //���
    private BigNumber baseArmor;
    private BigNumber accountArmor = 0;
    private BigNumber buildingArmor = 0;
    //ü��
    private BigNumber baseMaxHp;
    private BigNumber accountHp = 0;
    private BigNumber buildingHp = 0;
    //ũȮ
    private float accountCriticalChance = 0f;
    private float buildingCriticalChance = 0f;


    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        dealerSkill = stageManager.UnitPartyManager.GetUnit(UnitTypes.Dealer).GetComponent<DealerSkill>();
    }

    private void Start()
    {
    }
    public void AddStats(UpgradeType type , float amount)
    {
        switch (type)
        {
            case UpgradeType.AttackPoint:
                accountDamage += (int)amount;
                break;
            case UpgradeType.HealthPoint:
                accountHp += (int)amount;
                break;
            case UpgradeType.DefensePoint:
                accountArmor += (int)amount;
                break;
            case UpgradeType.CriticalPossibility:
                accountCriticalChance += (int)amount;
                break;
            case UpgradeType.CriticalDamages:
                accountCriticalDamage += (int)amount;
                break;
        }
    }

    public void AddBuildingStats(BuildingTable.BuildingType type , float amount)
    {
        if(amount == 0)
        {
            return;
        }

        switch (type)
        {
            case BuildingTable.BuildingType.IdleTime:
                break;
            case BuildingTable.BuildingType.AttackPoint:
                buildingAttackDamage += (int)amount;
                break;
            case BuildingTable.BuildingType.HealthPoint:
                maxHp += (int)amount;
                break;
            case BuildingTable.BuildingType.DefensePoint:
                armor += (int)amount;
                break;
            case BuildingTable.BuildingType.CriticalPossibility:
                buildingCriticalChance += (int)amount;
                break;
            case BuildingTable.BuildingType.CriticalDamages:
                buildingCriticalDamage += (int)amount;
                break;
            case BuildingTable.BuildingType.Gold:
                break;
            case BuildingTable.BuildingType.Mining:
                break;
        }
    }



    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = (int)data.MoveSpeed;
        baseDamage = data.Attack;

        baseMaxHp = int.Parse(data.HP);
        currentGrade = data.Grade;
        Hp = maxHp;

        coolDown = 1;
        baseArmor = int.Parse(data.Defence);
        range = (int)data.Range;

        switch (type)
        {
            case UnitTypes.Tanker:
                break;
            case UnitTypes.Dealer:
                break;
            case UnitTypes.Healer:
                break;
        }
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
            BigNumber criticalBase = (defalutValue * (baseDamage * 2f));
            BigNumber criticalBonus = (defalutValue * (accountCriticalDamage + buildingCriticalDamage));

            finialDamage = criticalBase + criticalBonus;
        }
        attack.damage = finialDamage;


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
        Attack attack = CreateSkillAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }

    public Attack CreateSkillAttack(CharacterStats defenderStats)
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
            BigNumber criticalBase = (defalutValue * (baseDamage * 2f));
            BigNumber criticalBonus = (defalutValue * (accountCriticalDamage + buildingCriticalDamage));

            var percent = criticalBase + criticalBonus;
            var criticalSkillDamage = finialDamage * percent;
        }
        attack.damage = finialSkillDamage;


        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }


}
