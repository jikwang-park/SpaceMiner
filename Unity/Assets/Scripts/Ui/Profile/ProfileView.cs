using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileView : MonoBehaviour
{
    [SerializeField]
    private UnitTypes type;

    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI defenseText;
    [SerializeField]
    private TextMeshProUGUI criticalPossibilityText;
    [SerializeField]
    private TextMeshProUGUI criticalMultiplierText;


    private void Start()
    {
        UnitCombatPowerCalculator.onCombatPowerChanged += OnCombatPowerChanged;
        StatusUpdate();
    }

    private void OnCombatPowerChanged()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        StatusUpdate();
    }

    private void OnEnable()
    {
        StatusUpdate();
    }

    private void StatusUpdate()
    {
        var stats = UnitCombatPowerCalculator.GetUnitCombatStats(type);
        attackText.text = stats.soldierAttack.ToString();
        hpText.text = stats.soldierHp.ToString();
        defenseText.text = stats.soldierArmor.ToString();
        criticalPossibilityText.text = stats.criticalPossibility.ToString();
        criticalMultiplierText.text = stats.criticalMultiplier.ToString();
    }
}
