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
    [SerializeField]
    private AddressableImage needItemImage;
    [SerializeField]
    private TextMeshProUGUI needItemCountText;

    //임시
    [SerializeField]
    private List<Sprite> skillImage = new List<Sprite>();

    private int needItemId;
    private int needItemCount;

    private int currentId;
    private int nextId;
    private UnitTypes currentType;
    [SerializeField]
    public Grade currentGrade;

    private int level;
    private bool isMax = false;

    private SkillUpgradeTable.Data data;

  
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        
    }
    private void Start()
    {
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
    }
    public void ShowFirstOpened(int id, UnitTypes type, Grade grade)
    {
        SetBoardText(id, type, grade);
    }

    public void SetImage(UnitTypes type)
    {
        int index = (int)type;

        currentImage.sprite = skillImage[index - 1];
        nextImage.sprite = skillImage[index - 1];

    }

    

    public void SetLimit(Grade grade, UnitTypes type, int id)
    {
        currentId = id;
        manager.unitSkillDictionary[type][grade] = currentId;
        //DataTableManager.SkillUpgradeTable.GetData(id).level 

    }

    public void SetBoardText(int id, UnitTypes type,Grade grade)
    {
        currentId = id;
        currentType = type;
        currentGrade = grade;
        SetImage(currentType);


        switch (type)
        {
            case UnitTypes.Tanker:
                var tankerData = DataTableManager.TankerSkillTable.GetData(id);
                var stringId = tankerData.DetailStringID;
                level = tankerData.Level;
                currentText.SetString(stringId, tankerData.Duration.ToString(), (tankerData.ShieldRatio * 100).ToString(), tankerData.CoolTime.ToString());
                break;
            case UnitTypes.Dealer:
                var dealerData = DataTableManager.DealerSkillTable.GetData(id);
                var dealerStringId = dealerData.DetailStringID;
                level = dealerData.Level;
                currentText.SetString(dealerStringId, dealerData.MonsterMaxTarget.ToString(), (dealerData.DamageRatio * 100).ToString(), dealerData.CoolTime.ToString());

                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                var healerStringId = healerData.DetailStringID;
                level = healerData.Level;
                currentText.SetString(healerStringId, (healerData.HealRatio * 100).ToString(), healerData.CoolTime.ToString());

                break;
        }

        if (level >= maxLevel)
        {
            nextText.SetString(60010);
            upgradeButton.interactable = false;

            return;
        }
        var data = DataTableManager.SkillUpgradeTable.GetData(currentId);

        this.data = data;
        needItemId = data.NeedItemID;
        needItemCount = int.Parse(data.NeedItemCount);
        var itemsprtieId = DataTableManager.ItemTable.GetData(needItemId).SpriteID;
        needItemImage.SetSprite(itemsprtieId);
        needItemCountText.text = $"{needItemCount}개 필요합니다";

        upgradeButton.interactable = true;


        switch (type)
        {
            case UnitTypes.Tanker:
                nextId = data.SkillPaymentID;
                var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                var nextstringId = nextTankerData.DetailStringID;
                nextText.SetString(nextstringId, nextTankerData.Duration.ToString(), (nextTankerData.ShieldRatio * 100).ToString(), nextTankerData.CoolTime.ToString());
                break;
            case UnitTypes.Dealer:
                nextId = data.SkillPaymentID;
                var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                var nextdealerStringId = nextDealerData.DetailStringID;
                nextText.SetString(nextdealerStringId, nextDealerData.MonsterMaxTarget.ToString(), (nextDealerData.DamageRatio * 100).ToString(), nextDealerData.CoolTime.ToString());
                break;
            case UnitTypes.Healer:
                nextId = data.SkillPaymentID;
                var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                var nextHealerStringId = nextHealerData.DetailStringID;
                nextText.SetString(nextHealerStringId, (nextHealerData.HealRatio * 100).ToString(), nextHealerData.CoolTime.ToString());
                break;
        }
    }
    private void Update()
    {
        if (ItemManager.CanConsume(needItemId, needItemCount))
        {
            upgradeButton.interactable = true;

        }
        else
        {
            upgradeButton.interactable = false;
        }
    }



    private void OnClickUpgradeButton()
    {
        ItemManager.ConsumeItem(needItemId, needItemCount);
        currentId = nextId;
        SetBoardText(currentId, currentType,currentGrade);
        stageManager.UnitPartyManager.UpgradeSkillStats(currentId, currentType);
        SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[currentType][currentGrade] = currentId;
        SaveLoadManager.SaveGame();
    }

}
