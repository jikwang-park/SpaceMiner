using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnitUpgradeTable;


public class UnitStats : CharacterStats
{
    public float attackSpeed { get; private set; } = 0;
    private StageManager stageManager;

    private Grade currentGrade;
    private float criticalPercent;

    public BigNumber FinialDamage
    {
        get
        {
            return ((baseDamage + accountDamage) * (1f + buildingAttackDamage));
        }
    }

    private BigNumber defalutValue = 1;
    //�⺻
    public BigNumber baseDamage { get; private set; } = 0;
    public BigNumber accountDamage { get; private set; } = 0;
    public float buildingAttackDamage { get; private set; } = 0;
    //ũ��
    public float accountCriticalDamage { get; private set; } = 0;
    public float buildingCriticalDamage { get; private set; } = 0;
    //���?
    public BigNumber baseArmor { get; private set; } = 0;
    public BigNumber accountArmor { get; private set; } = 0;
    public float buildingArmor { get; private set; } = 0;
    //ü��
    public BigNumber baseMaxHp { get; private set; } = 0;
    public BigNumber accountHp { get; private set; } = 0;
    public float buildingHp { get; private set; } = 0;
    //ũȮ
    public float accountCriticalChance { get; private set; } = 0;
    public float buildingCriticalChance { get; private set; } = 0;


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
    public static float GetCriticalStats(UnitTypes unitType, UpgradeType upgradeType)
    {
        int unitId = InventoryManager.GetInventoryData(unitType).equipElementID;
        var unitData = DataTableManager.SoldierTable.GetData(unitId);

        switch (upgradeType)
        {
            case UpgradeType.CriticalPossibility:
                float possibility = 0f;
                int accountCriticalPossibilityUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountCriticalPossibilityStat = GetAccountCriticalStat(upgradeType, accountCriticalPossibilityUpgradeLevel);

                var buildingCriticalPossibilityDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.CriticalPossibility);
                var buildingCriticalPossibilityLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.CriticalPossibility];
                var buildingCriticalPossibilityStats = buildingCriticalPossibilityDatas[buildingCriticalPossibilityLevel].Value;

                possibility = accountCriticalPossibilityStat + buildingCriticalPossibilityStats;
                return possibility;
            case UpgradeType.CriticalDamages:
                float criticalMultiplier = 0f;
                int accountCriticalMulUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountCriticalMulStat = GetAccountCriticalStat(upgradeType, accountCriticalMulUpgradeLevel);

                var buildingCriticalMulDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.CriticalDamages);
                var buildingCriticalMulLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.CriticalDamages];
                var buildingCriticalMulStats = buildingCriticalMulDatas[buildingCriticalMulLevel].Value;

                criticalMultiplier = 2f + accountCriticalMulStat + buildingCriticalMulStats;
                return criticalMultiplier;
        }
        return 0;
    }
    private static float GetAccountCriticalStat(UpgradeType upgradeType, int level)
    {
        float stat = 0;
        var data = DataTableManager.UnitUpgradeTable.GetData(upgradeType);
        for (int i = 1; i <= level; i++)
        {
            stat += data.Value * level;
        }
        return stat;
    }
    public static BigNumber GetStats(UnitTypes unitType, UpgradeType upgradeType)
    {
        int unitId = InventoryManager.GetInventoryData(unitType).equipElementID;
        var unitData = DataTableManager.SoldierTable.GetData(unitId);

        switch (upgradeType)
        {
            case UpgradeType.AttackPoint:
                BigNumber attackStat = 0;
                int accountAttackUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountAttackStat = GetAccountStat(upgradeType, accountAttackUpgradeLevel);

                var buildingAttackDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.AttackPoint);
                var buildingAttackLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.AttackPoint];
                var buildingAttackStats = buildingAttackDatas[buildingAttackLevel].Value;

                attackStat = (unitData.Attack + accountAttackStat) * (1 + buildingAttackStats);
                return attackStat;
            case UpgradeType.HealthPoint:
                BigNumber hpStat = 0;
                int accountHpUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountHpStat = GetAccountStat(upgradeType, accountHpUpgradeLevel);

                var buildingHpDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.HealthPoint);
                var buildingHpLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.HealthPoint];
                var buildingHpStats = buildingHpDatas[buildingHpLevel].Value;

                hpStat = (unitData.HP + accountHpStat) * (1 + buildingHpStats);
                return hpStat;
            case UpgradeType.DefensePoint:
                BigNumber armorStat = 0;
                int accountArmorUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountArmorStat = GetAccountStat(upgradeType, accountArmorUpgradeLevel);

                var buildingArmorDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.DefensePoint);
                var buildingArmorLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.DefensePoint];
                var buildingArmorStats = buildingArmorDatas[buildingArmorLevel].Value;

                armorStat = (unitData.Defence + accountArmorStat) * (1 + buildingArmorStats);
                return armorStat;
        }
        return 0;
    }
    private static BigNumber GetAccountStat(UpgradeType upgradeType, int level)
    {
        BigNumber stat = 0;
        var data = DataTableManager.UnitUpgradeTable.GetData(upgradeType);
        for(int i = 1; i <= level; i++)
        {
            stat += data.Value * level;
        }
        return stat;
    }
    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = data.MoveSpeed;
        baseDamage = data.Attack;
        attackSpeed = data.AttackSpeed;

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
    public void SkillExecute(GameObject defender) // ��ų ������ �����? ���� �Ѱܼ� ������ ó��
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
