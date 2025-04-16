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
    private LocalizationText buildingText;
    [SerializeField]
    private TextMeshProUGUI upgradeButtonCountText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Image lockedImage;
    [SerializeField]
    private LocalizationText buildingName;
    [SerializeField]
    private LocalizationText currentValueText;
    [SerializeField]
    private LocalizationText nextValueText;


    [SerializeField]
    private float nextValue;
    [SerializeField]
    private int level;
    [SerializeField]
    private int nextLevel;
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
    private AddressableImage buildingImage;

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
        SetLevelData(level);
    }

    private void SetBuildingName(BuildingTable.BuildingType type , int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        var nameId = data[level].NameStringID;
        buildingName.SetString(nameId);
    }
    
    private void SetBuildingExplanText(BuildingTable.BuildingType type , int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        var buildingExplanId = data[level].DetailStringID;
        currentValueText.SetString(buildingExplanId, (data[level].Value).ToString());
        if (level <= maxLevel - 1)
        {
            var bulidingExplanNextId = data[level + 1].DetailStringID;
            nextValueText.SetString(bulidingExplanNextId, (data[level + 1].Value).ToString());
        }
        else
        {
            nextValueText.SetString(60010);
        }

    }

    private void SetNextLevel(int level)
    {
        nextLevel = level + 1;
    }
   
    public void SetLevelData(int level)
    {
        if (IsMaxLevel(level))
        {
            SetMaxLevel(level);
            return;
        }

        id = data[level].ID;
        this.level = data[level].Level;
        value = data[level].Value;
        itemId = data[level].NeedItemID;
        maxLevel = data[level].MaxLevel;
        needItemCount = data[level].NeedItemCount;
        currentType = data[level].Type;
        SetNextLevel(level);
      

        nextValue = data[nextLevel].Value;
        if (level == 0)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
        SetFirstUpgrade(isLocked);
        SetConstructionInfo(level,currentType);
        SetBuildingExplanText(currentType, level);
    }

    private void SetMaxLevel(int level)
    {
        id = data[level].ID;
        this.level = data[level].Level;
        value = data[level].Value;
        itemId = data[level].NeedItemID;
        maxLevel = data[level].MaxLevel;
        needItemCount = data[level].NeedItemCount;
        currentType = data[level].Type;
        SetConstructionInfo(level, currentType);
        SetBuildingExplanText(currentType, level);
    }

    private void SetConstructionInfo(int level,BuildingTable.BuildingType type)
    {
        SetLevelText(level);
        SetCountText(type);
    }

    private void SetLevelText(int level)
    {
        levelText.text = $"LV.{level}";
    }
    private void SetBuildingText(BuildingTable.BuildingType type)
    {
        //나중에 추가해줘야됌 테이블
        buildingText.SetString(90);
    }
    private void SetCountText(BuildingTable.BuildingType type)
    {
        if (level <= maxLevel - 1)
        {
            upgradeButtonCountText.text = $"{needItemCount}";
        }
        else
        {
            upgradeButtonCountText.text = $"최대레벨";
        }

    }
    public void SetData(BuildingTable.BuildingType type,int level)
    {
        currentType = type;
        this.level = level;
        SetLevelData(level);
        SetImage(type, level);
        SetBuildingName(type, level);
        SetBuildingText(type);
        SetBuildingExplanText(type, level);
    }

    public float GetCurrentValue()
    {
        return 0;
    }

    public void LevelUp()
    {
        if (level > maxLevel)
            return;

        level++;
        
        SetLevelData(level);
        SetLevelText(level);

        stageManager.UnitPartyManager.AddBuildingStats(currentType,value);
        SetConstructionInfo(level, currentType);

        SaveLoadManager.Data.buildingData.buildingLevels[currentType] = level;
        SaveLoadManager.SaveGame();
    }

    public void SetImage(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        var spriteId = data[level].SpriteID;
        buildingImage.SetSprite(spriteId);
    }
   
    private bool IsMaxLevel(int level)
    {
        if(level >= maxLevel)
        {
            return true;

        }
        return false;
    }

    private void Update()
    {
        if(IsMaxLevel(level))
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
