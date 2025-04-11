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

    [SerializeField]
    public Button healerHpOptionButton;
    [SerializeField]
    public Slider healthSlider;

    public bool IsClicked = false;

    private const string Auto = "�ڵ�";
    private const string Manual = "����";
 
    [SerializeField]
    private Toggle autoToggle;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    public float currentValue { get; private set; }
   
    
    private StageManager stageManager;


    private void Awake()
    {
        healthSlider.gameObject.SetActive(false);
        healerHpOptionButton.onClick.AddListener(() => OnClickHealthSliderButton());
        healthSlider.onValueChanged.AddListener(OnHealthSilderholdChanaged);
        healthSlider.value = 0.5f;
        OnHealthSilderholdChanaged(healthSlider.value);
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.UnitPartyManager.OnUnitCreated += Init;
    }
    private void OnHealthSilderholdChanaged(float value)
    {
        currentValue = value * 100;
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

    private void OnClickHealthSliderButton()
    {
        IsClicked = !IsClicked;
        healthSlider.gameObject.SetActive(IsClicked);
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
