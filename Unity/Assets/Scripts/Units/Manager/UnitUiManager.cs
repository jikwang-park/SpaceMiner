using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUiManager : MonoBehaviour
{
    private Dictionary<UnitTypes, Slider> hpDic = new Dictionary<UnitTypes, Slider>();

    [SerializeField]
    private StageManager stageManager;
    private UnitPartyManager unitPartyManager;

    [SerializeField]
    private Slider tankerSlider;
    [SerializeField]
    private Slider dealerSlider;
    [SerializeField]
    private Slider healerSlider;


    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void Start()
    {
        stageManager.UnitPartyManager.OnUnitCreated += DoUnitCreated;
    }

    public void DoUnitCreated()
    {
        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            var unit = stageManager.UnitPartyManager.GetUnit(type);
            switch (type)
            {
                case UnitTypes.Tanker:
                    unit.GetComponent<UnitStats>().onHpChanged += (amount) => { tankerSlider.value = amount; };
                    break;
                case UnitTypes.Dealer:
                    unit.GetComponent<UnitStats>().onHpChanged += (amount) => { dealerSlider.value = amount; };

                    break;
                case UnitTypes.Healer:
                    unit.GetComponent<UnitStats>().onHpChanged += (amount) => { healerSlider.value = amount; };

                    break;
            }
        }
    }

}
