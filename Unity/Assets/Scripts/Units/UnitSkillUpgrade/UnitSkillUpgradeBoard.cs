using JetBrains.Annotations;
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

    [SerializeField]
    private TextMeshProUGUI needText;
    private int maxLevel = 20;

    //юс╫ц
    [SerializeField]
    private List<Sprite> skillImage = new List<Sprite>();


    private int currentId;
    private int nextId;
    private UnitTypes currentType;
    [SerializeField]
    public Grade currentGrade;

    private int level;
    private bool isMax = false;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }
    private void Start()
    {
          upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());

    }
    public void ShowFirstOpened(int id, UnitTypes type)
    {
        SetInfo(id, type);
    }

    public void SetImage(UnitTypes type)
    {
        int index = (int)type;

        currentImage.sprite = skillImage[index - 1];
        nextImage.sprite = skillImage[index - 1];

    }

    public void SetInfo(int id, UnitTypes type)
    {
        currentId = id;
        currentType = type;
        SetBoardText(id, type);
    }

    public void SetLimit(Grade grade, UnitTypes type, int id)
    {
        currentId = id;
        manager.unitSkillDictionary[type][grade] = currentId;
        //DataTableManager.SkillUpgradeTable.GetData(id).level 

    }

    public void SetBoardText(int id, UnitTypes type)
    {
        currentId = id;
        currentType = type;
        SetImage(currentType);
        var data = DataTableManager.SkillUpgradeTable.GetData(id);
        upgradeButton.interactable = true;

        switch (type)
        {
            case UnitTypes.Tanker:
                var tankerData = DataTableManager.TankerSkillTable.GetData(id);
                var stringId = tankerData.DetailStringID;
                level = tankerData.Level;
                currentText.SetString(stringId, tankerData.Duration.ToString(), (tankerData.ShieldRatio * 100).ToString(), tankerData.CoolTime.ToString());
                if (level >= maxLevel)
                {
                    nextText.SetString(60010);
                    upgradeButton.interactable = false;

                }
                else
                {
                    nextId = data.SkillPaymentID;
                    var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                    var nextstringId = nextTankerData.DetailStringID;
                    nextText.SetString(nextstringId, nextTankerData.Duration.ToString(), (nextTankerData.ShieldRatio * 100).ToString(), nextTankerData.CoolTime.ToString());
                }
                
                break;
            case UnitTypes.Dealer:
                var dealerData = DataTableManager.DealerSkillTable.GetData(id);
                var dealerStringId = dealerData.DetailStringID;
                level = dealerData.Level;
                currentText.SetString(dealerStringId, dealerData.MonsterMaxTarget.ToString(), (dealerData.DamageRatio * 100).ToString(), dealerData.CoolTime.ToString());
                if (level >= maxLevel)
                {
                    nextText.SetString(60010);
                    upgradeButton.interactable = false;

                }
                else
                {
                    nextId = data.SkillPaymentID;
                    var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                    var nextdealerStringId = nextDealerData.DetailStringID;
                    nextText.SetString(nextdealerStringId, nextDealerData.MonsterMaxTarget.ToString(), (nextDealerData.DamageRatio * 100).ToString(), nextDealerData.CoolTime.ToString());
                }
                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                var healerStringId = healerData.DetailStringID;
                level = healerData.Level;
                currentText.SetString(healerStringId, (healerData.HealRatio*100).ToString(), healerData.CoolTime.ToString());
                if (level >= maxLevel)
                {
                    nextText.SetString(60010);
                    upgradeButton.interactable = false;

                }
                else
                {
                    nextId = data.SkillPaymentID;
                    var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                    var nextHealerStringId = nextHealerData.DetailStringID;
                    nextText.SetString(nextHealerStringId, (nextHealerData.HealRatio*100).ToString(), nextHealerData.CoolTime.ToString());
                }
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

}
