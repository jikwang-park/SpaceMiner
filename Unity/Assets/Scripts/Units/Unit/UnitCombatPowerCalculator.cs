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

    public BigNumber CalculateTotalCombatPower()
    {
        return GetTankerCombatPower() + GetDealerCombatPower() + GetHealerCombatPower();
    }
    public BigNumber GetTankerCombatPower()
    {
        Unit tankerUnit = unitPartyManager.GetUnit(UnitTypes.Tanker).GetComponent<Unit>();
        return 0;
    }
    public BigNumber GetDealerCombatPower()
    {
        return 0;
    }
    public BigNumber GetHealerCombatPower()
    {
        return 0;
    }
}
