using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerLevel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI powerText;

    private void Start()
    {
        UnitCombatPowerCalculator.onCombatPowerChanged += CombatPowerChange;
        CombatPowerChange();
    }

    private void CombatPowerChange()
    {
        powerText.text = UnitCombatPowerCalculator.TotalCombatPower.ToString();
    }
}
