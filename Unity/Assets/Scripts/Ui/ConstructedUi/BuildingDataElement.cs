using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDataElement: MonoBehaviour
{
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private TextMeshProUGUI upgradeButtonText;
    [SerializeField]
    private Image constructionImage;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI constructionExplanText;
    [SerializeField]
    private Image lockedImage;


    [SerializeField]
    private int level;
    [SerializeField]
    private int id;
    [SerializeField]
    private float value;
    [SerializeField]
    private int itemId;
    [SerializeField]
    private int maxLevel;
    [SerializeField]
    private int uisequence;
    [SerializeField]
    private int needItemCount;
    [SerializeField]
    private bool isLocked = true;
    [SerializeField]
    private float currentValue = 0;

    [SerializeField]
    private BuildingTable.BuildingType currentType;

    private List<BuildingTable.Data> data;

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
    }

    public void Init(List<BuildingTable.Data> data)
    {
        this.data = data;
        level = 0;
        SetLevelData(level);
    }

    public void SetLevelData(int level)
    {
        id = data[level].ID;
        this.level = data[level].Level;
        value = data[level].Value;
        itemId = data[level].NeedItemID;
        maxLevel = data[level].MaxLevel;
        needItemCount = data[level].NeedItemCount;
        currentType = data[level].Type;
        if(level == 0)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
        SetFirstUpgrade(isLocked);
        SetConstructionInfo();

    }

    public void GetCurrentSequence()
    {

    }

    private void SetConstructionInfo()
    {
        SetLevelText(level);
        SetConstructionExplanText(currentType);
    }

    private void SetLevelText(int level)
    {
        levelText.text = $"LV.{level}";
    }
    private void SetConstructionExplanText(BuildingTable.BuildingType type)
    {
        constructionExplanText.text = $"{type.ToString()} \n {itemId} 가 {needItemCount} 개 필요합니다";
    }
    public void SetData(BuildingTable.BuildingType type,int level)
    {
        currentType = type;
        this.level = level;
        SetLevelData(level);
    }

    public float GetCurrentValue()
    {
        return 0;
    }

    public void LevelUp()
    {
        if (level == maxLevel)
            return;

        level++;
        SetLevelData(level);
        SetLevelText(level);

        stageManager.UnitPartyManager.AddBuildingStats(currentType,value);
        SetConstructionInfo();

        SaveLoadManager.Data.buildingData.buildingLevels[currentType] = level;
        SaveLoadManager.SaveGame();
    }

   
    private bool IsMaxLevel(int level)
    {
        if(level == maxLevel)
        {
            upgradeButtonText.text = "최대레벨달성";
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (IsMaxLevel(level))
        {
            upgradeButton.interactable = false;
        }
    }

    private void SetFirstUpgrade(bool isLocked)
    {
        if(isLocked)
        {
            lockedImage.gameObject.SetActive(true);
        }
        else
        {
            lockedImage.gameObject.SetActive(false);
        }

    }
    private void OnClickUpgradeButton()
    {
        
        if(isLocked)
        {
            isLocked = false;
        }
       
        LevelUp();

    }
}
