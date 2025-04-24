using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private SerializedDictionary<Toggle, UnitTypes> toggles;

    private void Start()
    {
        UnitCombatPowerCalculator.onCombatPowerChanged += OnCombatPowerChanged;
        StatusUpdate();
    }

    private void OnCombatPowerChanged()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        StatusUpdate();
    }

    private void OnEnable()
    {
        StatusUpdate();
    }

    public void SetStatus(bool isOn)
    {
        if (!isOn)
        {
            return;
        }
        foreach (var toggle in toggles)
        {
            if (toggle.Key.isOn)
            {
                type = toggle.Value;
                StatusUpdate();
            }
        }
    }

    private void StatusUpdate()
    {
        var stats = UnitCombatPowerCalculator.GetUnitCombatStats(type);
        attackText.text = stats.soldierAttack.ToString();
        hpText.text = stats.soldierMaxHp.ToString();
        defenseText.text = stats.soldierDefense.ToString();
        criticalPossibilityText.text = stats.criticalPossibility.ToString("P2");
        criticalMultiplierText.text = stats.criticalMultiplier.ToString("P2");
    }
}
