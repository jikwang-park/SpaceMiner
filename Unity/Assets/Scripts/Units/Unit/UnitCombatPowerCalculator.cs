using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitCombatPowerCalculator
{
    private const int weightDivider = 100;
    private const int tankerSkillWeight = 10;
    private const int healerSkillWeight = 7;
    public static event Action onCombatPowerChanged;
    public struct UnitCombatStats
    {
        public BigNumber soldierAttack;  
        public BigNumber soldierArmor;   
        public BigNumber soldierHp;      
        public float criticalPossibility;
        public float criticalMultiplier; 
    }
    public static BigNumber ToTalCombatPower { get; private set; }
    public static void CalculateTotalCombatPower()
    {
        var calculatedCombatPower = GetTankerCombatPower() + GetDealerCombatPower() + GetHealerCombatPower();
        if(ToTalCombatPower != calculatedCombatPower)
        {
            ToTalCombatPower = calculatedCombatPower;
            onCombatPowerChanged?.Invoke();
        }
    }
    public static BigNumber GetTankerCombatPower()
    {
        BigNumber combatPower = 0;
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Tanker].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        TankerSkillTable.Data skillData = DataTableManager.TankerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = UnitStats.GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = UnitStats.GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = UnitStats.GetStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = UnitStats.GetCriticalStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = UnitStats.GetCriticalStats(UnitTypes.Tanker, UnitUpgradeTable.UpgradeType.CriticalDamages);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierBaseAttackSpeed);

        combatPower = (attackPowerPerSecond * (soldierBaseAttack / weightDivider)) + (soldierArmor * (soldierBaseArmor / weightDivider)) + ((soldierHp + (soldierArmor * skillData.ShieldRatio * tankerSkillWeight / skillData.CoolTime)) * (soldierBaseHp / weightDivider));

        return combatPower;
    }

    public static BigNumber GetDealerCombatPower()
    {
        BigNumber combatPower = 0;
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Dealer].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        DealerSkillTable.Data skillData = DataTableManager.DealerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = UnitStats.GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = UnitStats.GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalDamages);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierBaseAttackSpeed);

        BigNumber skillExpectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, true);

        combatPower = (attackPowerPerSecond + (skillExpectedAttack / skillData.CoolTime) * (soldierBaseAttack / weightDivider)) + (soldierArmor * (soldierBaseArmor / weightDivider)) + (soldierHp * (soldierBaseHp / 100));

        return combatPower;
    }
    public static BigNumber GetHealerCombatPower()
    {
        BigNumber combatPower = 0;
        SoldierTable.Data unitData = DataTableManager.SoldierTable.GetData(SaveLoadManager.Data.soldierInventorySaveData[UnitTypes.Healer].equipElementID);
        int skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unitData.UnitType][unitData.Grade];
        HealerSkillTable.Data skillData = DataTableManager.HealerSkillTable.GetData(skillId);

        BigNumber soldierBaseAttack = unitData.Attack;
        BigNumber soldierBaseArmor = unitData.Defence;
        BigNumber soldierBaseHp = unitData.HP;
        BigNumber soldierBaseAttackSpeed = unitData.AttackSpeed;

        BigNumber soldierAttack = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.AttackPoint);
        BigNumber soldierArmor = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.DefensePoint);
        BigNumber soldierHp = UnitStats.GetStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.HealthPoint);
        float soldierCriticalPossibility = UnitStats.GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        float soldierCriticalMultiplier = UnitStats.GetCriticalStats(UnitTypes.Healer, UnitUpgradeTable.UpgradeType.CriticalDamages);

        BigNumber expectedAttack = GetExpectedDamage(soldierAttack, soldierCriticalMultiplier, soldierCriticalPossibility, skillId, false);
        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(expectedAttack, soldierBaseAttackSpeed);

        combatPower = (attackPowerPerSecond * (soldierBaseAttack / weightDivider)) + (soldierArmor * (soldierBaseArmor / weightDivider)) + ((soldierHp + (soldierHp * skillData.HealRatio * healerSkillWeight / skillData.CoolTime)) * (soldierBaseHp / weightDivider));

        return combatPower;
    }
    public static UnitCombatStats GetUnitCombatStats(UnitTypes unitType)
    {
        UnitCombatStats unitCombatStats = new UnitCombatStats();

        unitCombatStats.soldierAttack = UnitStats.GetStats(unitType, UnitUpgradeTable.UpgradeType.AttackPoint);
        unitCombatStats.soldierArmor = UnitStats.GetStats(unitType, UnitUpgradeTable.UpgradeType.DefensePoint);
        unitCombatStats.soldierHp = UnitStats.GetStats(unitType, UnitUpgradeTable.UpgradeType.HealthPoint);
        unitCombatStats.criticalPossibility = UnitStats.GetCriticalStats(unitType, UnitUpgradeTable.UpgradeType.CriticalPossibility);
        unitCombatStats.criticalMultiplier = UnitStats.GetCriticalStats(unitType, UnitUpgradeTable.UpgradeType.CriticalDamages);

        return unitCombatStats;
    }
    public static BigNumber GetAttackPowerPerSecond(BigNumber expectedDamage, BigNumber attackSpeed)
    {
        return expectedDamage * (attackSpeed / 100);
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
}
