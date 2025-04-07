using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitSkillUpgradeBoard : MonoBehaviour
{
    private UnitSkillUpgradeManager manager;

    private UnitSkillUpgradeElement element;

    [SerializeField]
    private Image currentImage;
    [SerializeField]
    private TextMeshProUGUI currentText;
    [SerializeField]
    private Image nextImage;
    [SerializeField]
    private TextMeshProUGUI nextText;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private StageManager stageManager;


    private int currentId = 1201;
    private int nextId;
    private UnitTypes currentType = UnitTypes.Tanker;
    [SerializeField]
    public Grade currentGrade = Grade.Normal;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }
    private void Start()
    {
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
        
    }
    public void ShowFirstOpened()
    {
        SetBoardText(currentId, currentType);
    }
    public void ShowBoard()
    {

    }

    public void SetInfo(int id , UnitTypes type)
    {
        currentId = id;
        currentType = type;
        SetBoardText(id, type);
    }

    public void SetBoardText(int id, UnitTypes type)
    {
        currentId = id;
        currentType = type;
         var data = DataTableManager.SkillUpgradeTable.GetData(id);
        nextId = data.SkillPaymentID;
        switch (type)
        {
            case UnitTypes.Tanker:
                   var tankerData = DataTableManager.TankerSkillTable.GetData(id);
                currentText.text = $"쉴드량 : {tankerData.ShieldRatio}% 쿨타임 : {tankerData.CoolTime}초 지속시간: {tankerData.Duration}초";
                var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                nextText.text = $"쉴드량 : {nextTankerData.ShieldRatio}% 쿨타임 : {nextTankerData.CoolTime}초 지속시간 : {nextTankerData.Duration}초";
                break;
            case UnitTypes.Dealer:
                var delaerData = DataTableManager.DealerSkillTable.GetData(id);
                currentText.text = $"쿨타임 : {delaerData.CoolTime}초 몬스터 타겟수: {delaerData.MonsterMaxTarget}초";
                var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                nextText.text = $"쿨타임 :  {nextDealerData.CoolTime} 초 몬스터 타겟수:  {nextDealerData.MonsterMaxTarget}초";
                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                currentText.text = $"쿨타임 : {healerData.CoolTime}초";
                var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                nextText.text = $"쿨타임 : {nextHealerData.CoolTime}초";
                break;
        }

        

    }
    private void OnClickUpgradeButton()
    {
        currentId = nextId;
        SetBoardText(currentId, currentType);
        stageManager.UnitPartyManager.UpgradeSkillStats(nextId, currentType);
    }

    public void SetImage()
    {

    }
}
