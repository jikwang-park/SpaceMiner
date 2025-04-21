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
        stageManager.UnitPartyManager.OnUnitCreated += DoUnitCreated;
    }

    private void Start()
    {
    }

    public void DoUnitCreated()
    {
        Debug.Log("1");
        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            var unit = stageManager.UnitPartyManager.GetUnit(type);
            var stats = unit.GetComponent<UnitStats>();
            switch (type)
            {
                case UnitTypes.Tanker:
                    stats.onHpChanged += (amount) => { tankerSlider.value = amount; };
                    break;
                case UnitTypes.Dealer:
                    stats.onHpChanged += (amount) => { dealerSlider.value = amount; };

                    break;
                case UnitTypes.Healer:
                    stats.onHpChanged += (amount) => { healerSlider.value = amount; };

                    break;
            }
        }
    }

}
