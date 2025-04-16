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
    private TextMeshProUGUI upgradeBuildingText;
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
        var buildingExplanId = data[level].NameStringID;
        var bulidingExplanNextId = data[level + 1].NameStringID;
        currentValueText.SetString(buildingExplanId, (data[level].Value).ToString());
        nextValueText.SetString(bulidingExplanNextId, (data[level + 1].Value).ToString());
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
        nextValue = data[level + 1].Value;
        if (level == 0)
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

    private void SetValueText(int level)
    {

    }


    private void SetConstructionInfo()
    {
        SetLevelText(level);
        SetCountText(currentType);
    }

    private void SetLevelText(int level)
    {
        levelText.text = $"LV.{level}";
    }
    private void SetBuildingText(BuildingTable.BuildingType type)
    {
        //로컬라이제이션 텍스트로 변경처리 해야됌
        upgradeBuildingText.text = "건물";
    }
    private void SetCountText(BuildingTable.BuildingType type)
    {
        upgradeButtonCountText.text = $"{needItemCount}";
    }
    public void SetData(BuildingTable.BuildingType type,int level)
    {
        currentType = type;
        this.level = level;
        SetLevelData(level);
        SetImage(type, level);
        SetBuildingName(type, level);
        SetBuildingText(type);
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

    public void SetImage(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        var spriteId = data[level].SpriteID;
        buildingImage.SetSprite(spriteId);
    }
   
    private bool IsMaxLevel(int level)
    {
        if(level == maxLevel)
        {
            upgradeBuildingText.text = "최대레벨달성";
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
