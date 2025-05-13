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
    [SerializeField]
    private GameObject sliderGameObject;

    public bool IsClicked = false;

    private const string Auto = "자동";
    private const string Manual = "수동";

    [SerializeField]
    private Toggle autoToggle;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private TextMeshProUGUI healthSliderPercentageText;
    [SerializeField]
    public float currentValue { get; private set; }


    private StageManager stageManager;


    private void Awake()
    {
        sliderGameObject.gameObject.SetActive(false);
        healerHpOptionButton.onClick.AddListener(() => OnClickHealthSliderButton());
        healthSlider.onValueChanged.AddListener(OnHealthSilderholdChanaged);
        healthSlider.value = SaveLoadManager.Data.healerSkillSliderValue;
        OnHealthSilderholdChanaged(healthSlider.value);
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.UnitPartyManager.OnUnitCreated += Init;
        stageManager.UnitPartyManager.OnUnitUpdated += SetSkillImage;
    }

  
    private void OnHealthSilderholdChanaged(float value)
    {
        currentValue = value * 100;
        Variables.healerSkillHPRatio = value;
        if (healthSliderPercentageText != null)
        {
            healthSliderPercentageText.text = $"{currentValue:F0}%";
        }
        SaveLoadManager.Data.healerSkillSliderValue = value;
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

        autoToggle.isOn = Variables.isAutoSkillMode;
        
    }

    private void SetSkillImage(UnitTypes type, Grade grade)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                tankerSkillButtonUi.SetSkillImage(type, grade);
                break;
            case UnitTypes.Dealer:
                dealerSkillButton.SetSkillImage(type, grade);
                break;
            case UnitTypes.Healer:
                healerSkillButton.SetSkillImage(type, grade);
                break;
        }
    }


    private void OnClickHealthSliderButton()
    {
        IsClicked = !IsClicked;
        sliderGameObject.gameObject.SetActive(IsClicked);
    }
    private void Update()
    {
        OnAutoToggleChanaged(autoToggle.isOn);
    }
    public void OnAutoToggleChanaged(bool isOn)
    {
        Variables.isAutoSkillMode = isOn;
        if (isOn)
        {
            //TODO: 스트링테이블
            text.text = Auto;
        }
        else
        {
            text.text = Manual;
        }
    }


}
