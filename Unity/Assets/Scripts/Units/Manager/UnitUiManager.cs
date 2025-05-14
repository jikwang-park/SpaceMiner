using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUiManager : MonoBehaviour
{
    [SerializeField]
    private Slider tankerSlider;
    [SerializeField]
    private Slider dealerSlider;
    [SerializeField]
    private Slider healerSlider;
    public void SetUnitHpBar(UnitTypes type, UnitStats stats)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                stats.OnHpChanged += (amount) => { tankerSlider.value = amount; };
                tankerSlider.value = stats.HPRate;
                break;
            case UnitTypes.Dealer:
                stats.OnHpChanged += (amount) => { dealerSlider.value = amount; };
                dealerSlider.value = stats.HPRate;
                break;
            case UnitTypes.Healer:
                stats.OnHpChanged += (amount) => { healerSlider.value = amount; };
                healerSlider.value = stats.HPRate;
                break;
        }
    }
}
