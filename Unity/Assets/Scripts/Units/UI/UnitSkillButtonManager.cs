using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillButtonManager : MonoBehaviour
{


    private List<Unit> unitList = new List<Unit>();
    [SerializeField]
    private UnitSkillButtonUi tankerSkillButtonUi;
    [SerializeField]
    private UnitSkillButtonUi dealerSkillButton;
    [SerializeField]
    private UnitSkillButtonUi healerSkillButton;

    private const string Auto = "자동";
    private const string Manual = "수동";
 
    [SerializeField]
    private Toggle autoToggle;
    [SerializeField]
    private TextMeshProUGUI text;


    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.UnitPartyManager.OnUnitCreated += Init;
    }

    private void Init()
    {
        var tankerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Tanker).gameObject.GetComponent<Unit>();
        tankerSkillButtonUi.SetUnit(tankerUnit);
        var dealerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Dealer).gameObject.GetComponent<Unit>();
        dealerSkillButton.SetUnit(dealerUnit);
        var healerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Healer).gameObject.GetComponent<Unit>();
        healerSkillButton.SetUnit(healerUnit);

        unitList.Add(tankerUnit);
        unitList.Add(dealerUnit);
        unitList.Add(healerUnit);

        foreach (var unit in unitList)
        {
            if (unit.isAutoSkillMode)
            {
                autoToggle.isOn = true;
            }
        }
    }
    private void Update()
    {
        OnAutoToggleChanaged(autoToggle.isOn);
    }
    public void OnAutoToggleChanaged(bool isOn)
    {
        if(isOn)
        {
            text.text = Auto;
            foreach(var unit in unitList)
            {
                unit.isAutoSkillMode = true;
            }
        }
        else
        {
            text.text = Manual;
            foreach(var unit in unitList)
            {
                unit.isAutoSkillMode = false;
            }
        }
    }


}
