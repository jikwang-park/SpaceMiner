using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitSkillUpgradeBoard : MonoBehaviour
{
    [SerializeField]
    private UnitSkillUpgradePanel manager;

    private const string maxLevelText = "Max Level";

    [SerializeField]
    private LocalizationText currentLevelText;
    [SerializeField]
    private LocalizationText nextLevelText;

    [SerializeField]
    private LocalizationText currentText;
    [SerializeField]
    private LocalizationText nextText;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private StageManager stageManager;
    [SerializeField]
    private LocalizationText buttonText;

    private const int maxLevel = 20;
    [SerializeField]
    private AddressableImage needItemImage;
    [SerializeField]
    private TextMeshProUGUI needItemCountText;

    [SerializeField]
    private AddressableImage currentSkillImage;
    [SerializeField]
    private AddressableImage nextSkillImage;

    [SerializeField]
    private GameObject nextInfoGameobject;

    private int needItemId;
    private BigNumber needItemCount;

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

    public void SetImage(UnitTypes type, int id)
    {
        int spriteId = 0;
        switch (type)
        {
            case UnitTypes.Tanker:
                spriteId = DataTableManager.TankerSkillTable.GetData(id).SpriteID;
                currentSkillImage.SetSprite(spriteId);
                nextSkillImage.SetSprite(spriteId);
                break;
            case UnitTypes.Dealer:
                spriteId = DataTableManager.DealerSkillTable.GetData(id).SpriteID;
                currentSkillImage.SetSprite(spriteId);
                nextSkillImage.SetSprite(spriteId);
                break;
            case UnitTypes.Healer:
                spriteId = DataTableManager.HealerSkillTable.GetData(id).SpriteID;
                currentSkillImage.SetSprite(spriteId);
                nextSkillImage.SetSprite(spriteId);
                break;
        }
    }




    public void SetLimit(Grade grade, UnitTypes type, int id)
    {
        currentId = id;
        manager.unitSkillDictionary[type][grade] = currentId;
    }

    public void SetBoardText(int id, UnitTypes type, Grade grade)
    {
        currentId = id;
        currentType = type;
        currentGrade = grade;
        SetImage(currentType, id);


        SetCurrentSkillText(currentType, currentId, out level);

        if (level >= maxLevel)
        {
            SetMaxLevel();
            return;
        }


        LoadUpgradeData();
        SetNextSkillText(currentType, nextId);
        UpdateUpgradeUI();
    }


    private void SetCurrentSkillText(UnitTypes type, int id, out int level)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                var tankerData = DataTableManager.TankerSkillTable.GetData(id);
                var stringId = tankerData.DetailStringID;
                level = tankerData.Level;
                currentText.SetString(stringId, tankerData.Duration.ToString(), (tankerData.ShieldRatio * 100).ToString(), tankerData.CoolTime.ToString());
                SetLevelText(currentLevelText, level);
                break;
            case UnitTypes.Dealer:
                var dealerData = DataTableManager.DealerSkillTable.GetData(id);
                var dealerStringId = dealerData.DetailStringID;
                level = dealerData.Level;
                currentText.SetString(dealerStringId, dealerData.MonsterMaxTarget.ToString(), (dealerData.DamageRatio * 100).ToString(), dealerData.CoolTime.ToString());
                SetLevelText(currentLevelText, level);
                break;
            case UnitTypes.Healer:
                var healerData = DataTableManager.HealerSkillTable.GetData(id);
                var healerStringId = healerData.DetailStringID;
                level = healerData.Level;
                currentText.SetString(healerStringId, (healerData.HealRatio * 100).ToString(), healerData.CoolTime.ToString());
                SetLevelText(currentLevelText, level);
                break;
            default:
                level = 0;
                break;
        }
    }

    private void SetLevelText(LocalizationText text, int level)
    {
        if (level >= maxLevel)
        {
            text.SetStringArguments(maxLevel.ToString());
        }
        else
        {
            text.SetStringArguments(level.ToString());
        }
    }

    private void SetNextSkillText(UnitTypes type, int id)
    {
        switch (type)
        {
            case UnitTypes.Tanker:
                nextId = data.SkillPaymentID;
                var nextTankerData = DataTableManager.TankerSkillTable.GetData(nextId);
                var nextstringId = nextTankerData.DetailStringID;
                var tankerNextLevel = nextTankerData.Level;
                nextText.SetString(nextstringId, nextTankerData.Duration.ToString(), (nextTankerData.ShieldRatio * 100).ToString(), nextTankerData.CoolTime.ToString());
                SetLevelText(nextLevelText, tankerNextLevel);
                break;
            case UnitTypes.Dealer:
                nextId = data.SkillPaymentID;
                var nextDealerData = DataTableManager.DealerSkillTable.GetData(nextId);
                var nextdealerStringId = nextDealerData.DetailStringID;
                var dealerNextLevel = nextDealerData.Level;
                nextText.SetString(nextdealerStringId, nextDealerData.MonsterMaxTarget.ToString(), (nextDealerData.DamageRatio * 100).ToString(), nextDealerData.CoolTime.ToString());
                SetLevelText(nextLevelText, dealerNextLevel);
                break;
            case UnitTypes.Healer:
                nextId = data.SkillPaymentID;
                var nextHealerData = DataTableManager.HealerSkillTable.GetData(nextId);
                var nextHealerStringId = nextHealerData.DetailStringID;
                var healerNextLevel = nextHealerData.Level;
                nextText.SetString(nextHealerStringId, (nextHealerData.HealRatio * 100).ToString(), nextHealerData.CoolTime.ToString());
                SetLevelText(nextLevelText, healerNextLevel);
                break;
        }
    }

    private void SetMaxLevel()
    {
        upgradeButton.gameObject.SetActive(false);
        nextText.SetString(60010);
        nextInfoGameobject.SetActive(false);
        needItemImage.gameObject.SetActive(false);
        needItemCountText.text = maxLevelText;
    }
    private void UpdateUpgradeUI()
    {
        upgradeButton.gameObject.SetActive(true);
        nextInfoGameobject.SetActive(true);
        needItemImage.gameObject.SetActive(true);
        needItemImage.SetSprite(DataTableManager.ItemTable.GetData(needItemId).SpriteID);
        needItemCountText.text = $"{needItemCount}개 필요합니다";
    }

    private void LoadUpgradeData()
    {
        data = DataTableManager.SkillUpgradeTable.GetData(currentId);
        nextId = data.SkillPaymentID;
        needItemId = data.NeedItemID;
        needItemCount = data.NeedItemCount;
    }
    private void Update()
    {
        if (level >= maxLevel)
        {
            upgradeButton.interactable = false;
            return;
        }

        bool canUpgrade = ItemManager.CanConsume(needItemId, needItemCount);
        upgradeButton.interactable = canUpgrade;
        buttonText.SetColor(new Color(1f, 1f, 1f, canUpgrade ? 1f : 0.2f));
    }
    private void OnClickUpgradeButton()
    {
        ItemManager.ConsumeItem(needItemId, needItemCount);
        currentId = nextId;
        manager.unitSkillDictionary[currentType][currentGrade] = currentId;

        SetBoardText(currentId, currentType, currentGrade);
        stageManager.UnitPartyManager.UpgradeSkillStats(currentId, currentType);
        SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[currentType][currentGrade] = currentId;
        SaveLoadManager.SaveGame();
    }

}
