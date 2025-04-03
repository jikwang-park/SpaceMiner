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


    private int currentId = 1201;
    private int nextId;
    private UnitTypes currentType = UnitTypes.Tanker;
    [SerializeField]
    public Grade currentGrade = Grade.Normal;
    private void Start()
    {
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
        SetInfo(currentId, currentType);
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
        nextId = data.NextSkillUpgrade;
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
    }

    public void SetImage()
    {

    }
}
