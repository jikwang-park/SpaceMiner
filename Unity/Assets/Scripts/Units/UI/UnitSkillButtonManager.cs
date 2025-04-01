using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSkillButtonManager : MonoBehaviour
{

    private Dictionary<UnitTypes, UnitSkillButtonUi> skillButtons;
    [SerializeField]
    private UnitSkillButtonUi tankerSkillButtonUi;
    [SerializeField]
    private UnitSkillButtonUi dealerSkillButton;
    [SerializeField]
    private UnitSkillButtonUi healerSkillButton;


    private StageManager stageManager;

    private void Awake()
    {

    }



    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        //skillButtons = new Dictionary<UnitTypes, UnitSkillButtonUi>();
        //for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
        //{
        //    var skillButton = Instantiate(unitSkillButtonUi);
        //    var unit = stageManager.UnitPartyManager.GetUnit((UnitTypes)i).GetComponent<Unit>();
        //    skillButton.SetUnit(unit);
        //    skillButtons[(UnitTypes)i] = skillButton;
        //}
        
        var tankerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Tanker).gameObject.GetComponent<Unit>();
        tankerSkillButtonUi.SetUnit(tankerUnit);
        var dealerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Dealer).gameObject.GetComponent<Unit>();
        dealerSkillButton.SetUnit(dealerUnit);
        var healerUnit = stageManager.UnitPartyManager.GetUnit(UnitTypes.Healer).gameObject.GetComponent<Unit>();
        healerSkillButton.SetUnit(healerUnit);
    }




}
