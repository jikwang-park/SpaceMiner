using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatPowerCalculator : MonoBehaviour
{
    private UnitPartyManager unitPartyManager;
    private const int weightDivider = 100;
    private const int tankerSkillWeight = 10;
    private const int healerSkillWeight = 7;
    private BigNumber CombatPower
    {
        get
        {
            return CalculateTotalCombatPower();
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        unitPartyManager = GetComponent<StageManager>().UnitPartyManager;
    }
    private void Start()
    {
        CalculateTotalCombatPower();
    }

    public BigNumber CalculateTotalCombatPower()
    {
        return GetTankerCombatPower() + GetDealerCombatPower() + GetHealerCombatPower();
    }
    public BigNumber GetTankerCombatPower()
    {
        BigNumber combatPower = 0;
        Unit unit = unitPartyManager.GetUnit(UnitTypes.Tanker).GetComponent<Unit>();
        UnitStats unitStats = unit.unitStats;

        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(unit);
        TankerSkillTable.Data skillData = DataTableManager.TankerSkillTable.GetData(unit.unitSkill.currentSkillId);

        combatPower = (attackPowerPerSecond * (unitStats.baseDamage / weightDivider)) + (unitStats.armor * (unitStats.baseArmor / weightDivider)) + ((unitStats.maxHp + (unitStats.armor * skillData.ShieldRatio * tankerSkillWeight / skillData.CoolTime)) * (unitStats.baseMaxHp / weightDivider));

        return combatPower;
    }
    public BigNumber GetDealerCombatPower()
    {
        BigNumber combatPower = 0;
        Unit unit = unitPartyManager.GetUnit(UnitTypes.Dealer).GetComponent<Unit>();
        UnitStats unitStats = unit.unitStats;

        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(unit);
        DealerSkillTable.Data skillData = DataTableManager.DealerSkillTable.GetData(unit.unitSkill.currentSkillId);

        combatPower = (attackPowerPerSecond + (GetExpectedDamage(unit, true) / skillData.CoolTime) * (unitStats.baseDamage / weightDivider)) + (unitStats.baseArmor / weightDivider) + (unitStats.baseMaxHp / weightDivider);

        return combatPower;
    }
    public BigNumber GetHealerCombatPower()
    {
        BigNumber combatPower = 0;
        Unit unit = unitPartyManager.GetUnit(UnitTypes.Healer).GetComponent<Unit>();
        UnitStats unitStats = unit.unitStats;

        BigNumber attackPowerPerSecond = GetAttackPowerPerSecond(unit);
        HealerSkillTable.Data skillData = DataTableManager.HealerSkillTable.GetData(unit.unitSkill.currentSkillId);

        combatPower = (attackPowerPerSecond * (unitStats.baseDamage / weightDivider)) + (unitStats.armor * (unitStats.baseArmor / weightDivider)) + ((unitStats.maxHp + (unitStats.maxHp * skillData.HealRatio * healerSkillWeight / skillData.CoolTime)) * (unitStats.baseMaxHp / weightDivider));

        return combatPower;
    }
    public BigNumber GetAttackPowerPerSecond(Unit unit)
    {
        BigNumber attackPowerPerSecond = 0;

        BigNumber expectedNormalDamage = GetExpectedDamage(unit, false);

        attackPowerPerSecond = expectedNormalDamage * (unit.unitStats.attackSpeed / 100);

        return attackPowerPerSecond;
    }
    public BigNumber GetExpectedDamage(Unit unit, bool isDealerSkill)
    {
        BigNumber expectedDamage = 0;
        UnitStats unitStats = unit.unitStats;
        if (isDealerSkill)
        {
            UnitSkill unitSkill = unit.unitSkill;
            DealerSkillTable.Data skillData = DataTableManager.DealerSkillTable.GetData(unitSkill.currentSkillId);

            expectedDamage = (unitStats.FinialDamage * skillData.DamageRatio) * (1 - (unitStats.accountCriticalChance + unitStats.buildingCriticalChance)) + ((unitStats.FinialDamage * skillData.DamageRatio) * (2 + unitStats.accountCriticalDamage + unitStats.buildingCriticalDamage) * (unitStats.accountCriticalChance + unitStats.buildingCriticalChance));
        }
        else
        {
            expectedDamage = unitStats.FinialDamage * (1 - (unitStats.accountCriticalChance + unitStats.buildingCriticalChance)) + (unitStats.FinialDamage * (2 + unitStats.accountCriticalDamage + unitStats.buildingCriticalDamage) * (unitStats.accountCriticalChance + unitStats.buildingCriticalChance));
        }
        return expectedDamage;
    }
}
