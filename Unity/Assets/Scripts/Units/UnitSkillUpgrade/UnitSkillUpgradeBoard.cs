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
    private LocalizationText currentText;
    [SerializeField]
    private Image nextImage;
    [SerializeField]
    private LocalizationText nextText;
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
                var stringId = tankerData.DetailStringID;
                currentText.SetString(stringId);
                currentText.SetStringArguments(tankerData.Duration.ToString(),tankerData.ShieldRatio.ToString(),tankerData.CoolTime.ToString());
                var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                var nextstringId = nextTankerData.DetailStringID;
                nextText.SetString(nextstringId);
                nextText.SetStringArguments(nextTankerData.Duration.ToString(), nextTankerData.ShieldRatio.ToString(), nextTankerData.CoolTime.ToString());
                break;
            case UnitTypes.Dealer:
                var dealerData = DataTableManager.DealerSkillTable.GetData(id);
                var dealerStringId = dealerData.DetailStringID;
                currentText.SetString(dealerStringId);
                currentText.SetStringArguments(dealerData.MonsterMaxTarget.ToString(), dealerData.DamageRatio.ToString(), dealerData.CoolTime.ToString());
                var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                var nextdealerStringId = nextDealerData.DetailStringID;
                nextText.SetString(nextdealerStringId);
                nextText.SetStringArguments(nextDealerData.MonsterMaxTarget.ToString(), nextDealerData.DamageRatio.ToString(), nextDealerData.CoolTime.ToString());
                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                var healerStringId = healerData.DetailStringID;
                currentText.SetString(healerStringId);
                currentText.SetStringArguments(healerData.HealRatio.ToString(), healerData.CoolTime.ToString());
                var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                var nextHealerStringId = nextHealerData.DetailStringID;
                nextText.SetString(nextHealerStringId);
                nextText.SetStringArguments(nextHealerData.HealRatio.ToString(), nextHealerData.CoolTime.ToString());
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
