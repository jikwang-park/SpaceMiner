using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitSkillUpgradeBoard : MonoBehaviour
{
    private UnitSkillUpgradePanel manager;


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


    private int currentId;
    private int nextId;
    private UnitTypes currentType;
    [SerializeField]
    public Grade currentGrade;

    private int level;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }
    private void Start()
    {
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
        
    }
    public void ShowFirstOpened(int id , UnitTypes type)
    {
        SetInfo(id, type);
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

    public void SetLimit(Grade grade , UnitTypes type, int id)
    {
        currentId = id;
        manager.unitSkillDictionary[type][grade] = currentId;
        //DataTableManager.SkillUpgradeTable.GetData(id).level 

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
                currentText.text = $"���差 : {tankerData.ShieldRatio}% ��Ÿ�� : {tankerData.CoolTime}�� ���ӽð�: {tankerData.Duration}��";
                var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                nextText.text = $"���差 : {nextTankerData.ShieldRatio}% ��Ÿ�� : {nextTankerData.CoolTime}�� ���ӽð� : {nextTankerData.Duration}��";
                break;
            case UnitTypes.Dealer:
                var delaerData = DataTableManager.DealerSkillTable.GetData(id);
                currentText.text = $"��Ÿ�� : {delaerData.CoolTime}�� ���� Ÿ�ټ�: {delaerData.MonsterMaxTarget}��";
                var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                nextText.text = $"��Ÿ�� :  {nextDealerData.CoolTime} �� ���� Ÿ�ټ�:  {nextDealerData.MonsterMaxTarget}��";
                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                currentText.text = $"��Ÿ�� : {healerData.CoolTime}��";
                var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                nextText.text = $"��Ÿ�� : {nextHealerData.CoolTime}��";
                break;
        }
    }
    private void OnClickUpgradeButton()
    {
        currentId = nextId;
        SetBoardText(currentId, currentType);
        stageManager.UnitPartyManager.UpgradeSkillStats(currentId, currentType);
        SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[currentType][currentGrade] = currentId;
        SaveLoadManager.SaveGame();
    }

    public void SetImage()
    {

    }
}
