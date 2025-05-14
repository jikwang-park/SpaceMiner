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
    private Slider GetSlider(UnitTypes type)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                return tankerSlider;
            case UnitTypes.Dealer:
                return dealerSlider;
            case UnitTypes.Healer:
                return healerSlider;
            default:
                return null;
        }
    }
    public void SetUnitRevive(UnitTypes type)
    {
        var slider = GetSlider(type);
        if (slider != null)
        {
            slider.fillRect.GetComponent<Image>().enabled = true;
        }
    }
    public void SetUnitDead(UnitTypes type)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                tankerSlider.fillRect.GetComponent<Image>().enabled = false;
                break;
            case UnitTypes.Dealer:
                dealerSlider.fillRect.GetComponent<Image>().enabled = false;
                break;
            case UnitTypes.Healer:
                healerSlider.fillRect.GetComponent<Image>().enabled = false;
                break;
        }
    }
}
