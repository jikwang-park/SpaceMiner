using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitUpgradeTable;

public static class UnitCombatPowerCalculator
{
    private const int weightDivider = 100;
    private const int tankerSkillWeight = 10;
    private const int healerSkillWeight = 7;
    public static event Action onCombatPowerChanged;

    public static Dictionary<UnitTypes, UnitCombatStats> statsDictionary = new Dictionary<UnitTypes, UnitCombatStats>();
    public class UnitCombatStats
    {
        public BigNumber soldierAttack;
        public BigNumber soldierDefense;
        public BigNumber soldierMaxHp;
        public float criticalPossibility;
        public float criticalMultiplier;
        public float coolDown;
        public float moveSpeed;
        public float attackRange;
        public float addNormalDamage;
        public float addBossDamage;
    }
    public static BigNumber TotalCombatPower { get; private set; }
    static UnitCombatPowerCalculator()
    {
        CalculateTotalCombatPower();
    }
    public static void Clear()
    {
        onCombatPowerChanged = null;
    }
    public static void Init(UnitTypes type)
    {
        var stats = GetUnitCombatStats(type);
        if (statsDictionary.ContainsKey(type))
        {
            statsDictionary[type] = stats;
        }
        else
        {
            statsDictionary.Add(type, stats);
        }
    }

    public static void ChangeStats(UpgradeType upgradeType)
    {
        foreach (var stat in statsDictionary)
        {
            switch (upgradeType)
            {
                case UpgradeType.AttackPoint:
                    stat.Value.soldierAttack = GetStats(stat.Key, upgradeType);
                    break;
                case UpgradeType.HealthPoint:
                    stat.Value.soldierMaxHp = GetStats(stat.Key, upgradeType);
                    break;
                case UpgradeType.DefensePoint:
                    stat.Value.soldierDefense = GetStats(stat.Key, upgradeType);
                    break;
                case UpgradeType.CriticalPossibility:
                    stat.Value.criticalPossibility = GetCriticalStats(stat.Key, upgradeType);
                    break;
                case UpgradeType.CriticalDamages:
                    stat.Value.criticalMultiplier = GetCriticalStats(stat.Key, upgradeType);
                    break;
            }
        }
        onCombatPowerChanged?.Invoke();
    }
    public static void CalculateTotalCombatPower()
    {
        var calculatedCombatPower = GetTankerCombatPower() + GetDealerCombatPower() + GetHealerCombatPower();
        if (TotalCombatPower != calculatedCombatPower)
        {
            TotalCombatPower = calculatedCombatPower;
            onCombatPowerChanged?.Invoke();
        }
    }
    public static BigNumber GetTankerCombatPower()
    {
        BigNumber combatPower = 0;
        if (SaveLoadManager.Data == null)
        {
            SaveLoadManager.SetDefaultData();
        }
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Tanker].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        TankerSkillTable.Data skillData = DataTableManager.TankerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = GetCriticalStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = GetCriticalStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.CriticalDamages);
        float soldierAddNormalDamage = GetEffectItemStat(EffectItemTable.ItemType.NormalMonsterDamage);
        float soldierAddBossDamage = GetEffectItemStat(EffectItemTable.ItemType.BossMonsterDamage);
        BigNumber soldierAttackSpeed = soldierBaseAttackSpeed * GetEffectItemStat(EffectItemTable.ItemType.AttackSpeed);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierAttackSpeed, soldierAddNormalDamage, soldierAddBossDamage);

        combatPower = (attackPowerPerSecond.DivideToFloat(soldierBaseAttack) * (weightDivider) + (soldierArmor.DivideToFloat(soldierBaseArmor) * weightDivider) + ((soldierHp + (soldierArmor * skillData.ShieldRatio * tankerSkillWeight / skillData.CoolTime)).DivideToFloat(soldierBaseHp) * weightDivider));

        return combatPower;
    }

    public static BigNumber GetDealerCombatPower()
    {
        BigNumber combatPower = 0;
        if (SaveLoadManager.Data == null)
        {
            SaveLoadManager.SetDefaultData();
        }
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Dealer].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        DealerSkillTable.Data skillData = DataTableManager.DealerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = GetStats(UnitTypes.Dealer, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = GetStats(UnitTypes.Dealer, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = GetStats(UnitTypes.Dealer, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = GetCriticalStats(UnitTypes.Dealer, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = GetCriticalStats(UnitTypes.Dealer, UnitUpgradeTable.UpgradeType.CriticalDamages);
        float soldierAddNormalDamage = GetEffectItemStat(EffectItemTable.ItemType.NormalMonsterDamage);
        float soldierAddBossDamage = GetEffectItemStat(EffectItemTable.ItemType.BossMonsterDamage);
        BigNumber soldierAttackSpeed = soldierBaseAttackSpeed * GetEffectItemStat(EffectItemTable.ItemType.AttackSpeed);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierAttackSpeed, soldierAddNormalDamage, soldierAddBossDamage);

        BigNumber skillExpectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, true);

        combatPower = (attackPowerPerSecond + (skillExpectedAttack / skillData.CoolTime).DivideToFloat(soldierBaseAttack) * weightDivider) + (soldierArmor.DivideToFloat(soldierBaseArmor) * weightDivider) + (soldierHp.DivideToFloat(soldierBaseHp) * 100);

        return combatPower;
    }
    public static BigNumber GetHealerCombatPower()
    {
        BigNumber combatPower = 0;
        if (SaveLoadManager.Data == null)
        {
            SaveLoadManager.SetDefaultData();
        }
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Healer].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        HealerSkillTable.Data skillData = DataTableManager.HealerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalDamages);
        float soldierAddNormalDamage = GetEffectItemStat(EffectItemTable.ItemType.NormalMonsterDamage);
        float soldierAddBossDamage = GetEffectItemStat(EffectItemTable.ItemType.BossMonsterDamage);
        BigNumber soldierAttackSpeed = soldierBaseAttackSpeed * GetEffectItemStat(EffectItemTable.ItemType.AttackSpeed);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierAttackSpeed, soldierAddNormalDamage, soldierAddBossDamage);

        combatPower = (attackPowerPerSecond.DivideToFloat(soldierBaseAttack) * weightDivider) + (soldierArmor.DivideToFloat(soldierBaseArmor) * weightDivider) + ((soldierHp + (soldierHp * skillData.HealRatio * healerSkillWeight / skillData.CoolTime)).DivideToFloat(soldierBaseHp) * weightDivider);

        return combatPower;
    }
    public static UnitCombatStats GetUnitCombatStats(UnitTypes unitType)
    {
        UnitCombatStats unitCombatStats = new UnitCombatStats();

        int unitId = InventoryManager.GetInventoryData(unitType).equipElementID;
        var unitData = DataTableManager.SoldierTable.GetData(unitId);

        unitCombatStats.soldierAttack = GetStats(unitType, UnitUpgradeTable.UpgradeType.AttackPoint);
        unitCombatStats.soldierDefense = GetStats(unitType, UnitUpgradeTable.UpgradeType.DefensePoint);
        unitCombatStats.soldierMaxHp = GetStats(unitType, UnitUpgradeTable.UpgradeType.HealthPoint);
        unitCombatStats.criticalPossibility = GetCriticalStats(unitType, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        unitCombatStats.criticalMultiplier = GetCriticalStats(unitType, UnitUpgradeTable.UpgradeType.CriticalDamages);
        unitCombatStats.coolDown = 100f / (unitData.AttackSpeed * GetEffectItemStat(EffectItemTable.ItemType.AttackSpeed)); 
        unitCombatStats.moveSpeed = unitData.MoveSpeed;
        unitCombatStats.attackRange = unitData.Range;
        unitCombatStats.addNormalDamage = GetEffectItemStat(EffectItemTable.ItemType.NormalMonsterDamage);
        unitCombatStats.addBossDamage = GetEffectItemStat(EffectItemTable.ItemType.BossMonsterDamage);

        return unitCombatStats;
    }
    public static BigNumber GetAttackPowerPerSecond(BigNumber expectedDamage, BigNumber attackSpeed, float addNormalDamage, float addBossDamage)
    {
        return expectedDamage * (attackSpeed / 100) *  (1 + (addNormalDamage + addBossDamage) / 2);
    }
    public static BigNumber GetExpectedDamage(BigNumber normalDamage, float criticalMul, float criticalPossibility, int skillId, bool isDealerSkill)
    {
        BigNumber expectedDamage = 0;
        if (isDealerSkill)
        {
            var unitSkill = DataTableManager.DealerSkillTable.GetData(skillId);

            expectedDamage = (normalDamage * unitSkill.DamageRatio) * (1 - criticalPossibility) + (normalDamage * criticalMul * unitSkill.DamageRatio) * criticalPossibility;
        }
        else
        {
            expectedDamage = normalDamage * (1 - criticalPossibility) + (normalDamage * criticalMul) * criticalPossibility;
        }
        return expectedDamage;
    }
    public static float GetEffectItemStat(EffectItemTable.ItemType type)
    {
        int effectItemLevel = EffectItemInventoryManager.GetLevel(type);
        float effectItemValue = DataTableManager.EffectItemTable.GetDatas(type)[effectItemLevel].Value;
        return (1 + effectItemValue);
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
        stat = data.Value * level;
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

                var attackEffectItemStats = GetEffectItemStat(EffectItemTable.ItemType.Attack);

                attackStat = (unitData.Attack + accountAttackStat) * (1 + buildingAttackStats) * attackEffectItemStats;
                return attackStat;
            case UpgradeType.HealthPoint:
                BigNumber hpStat = 0;
                int accountHpUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountHpStat = GetAccountStat(upgradeType, accountHpUpgradeLevel);

                var buildingHpDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.HealthPoint);
                var buildingHpLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.HealthPoint];
                var buildingHpStats = buildingHpDatas[buildingHpLevel].Value;

                var hpEffectItemStats = GetEffectItemStat(EffectItemTable.ItemType.HP);

                hpStat = (unitData.HP + accountHpStat) * (1 + buildingHpStats) * hpEffectItemStats;
                return hpStat;
            case UpgradeType.DefensePoint:
                BigNumber armorStat = 0;
                int accountArmorUpgradeLevel = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[upgradeType];
                var accountArmorStat = GetAccountStat(upgradeType, accountArmorUpgradeLevel);

                var buildingArmorDatas = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.DefensePoint);
                var buildingArmorLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.DefensePoint];
                var buildingArmorStats = buildingArmorDatas[buildingArmorLevel].Value;

                var armorEffectItemStats = GetEffectItemStat(EffectItemTable.ItemType.Defence);

                armorStat = (unitData.Defence + accountArmorStat) * (1 + buildingArmorStats) * armorEffectItemStats;
                return armorStat;
        }
        return 0;
    }
    private static BigNumber GetAccountStat(UpgradeType upgradeType, int level)
    {
        if(upgradeType == UpgradeType.AttackPoint)
        {
            return GetAccountUpgradeAttackStat(level);
        }

        BigNumber stat = 0;
        var data = DataTableManager.UnitUpgradeTable.GetData(upgradeType);

        stat = data.Value * level;
        
        return stat;
    }

    public static BigNumber GetAccountUpgradeAttackStat(int level)
    {
        BigNumber stat = 0;
        int value = 3;
        const int per = 100;
        while (level - per >= 0)
        {
            if (level >= per)
            {
                stat += per * value;
                level -= per;
            }
            value++;
        }
        
        stat += level * value;
        return stat;
    }
}
