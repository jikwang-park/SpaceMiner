using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDataElement : MonoBehaviour
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
    //[SerializeField]
    //private AddressableImage lockdImage;
    [SerializeField]
    private LocalizationText buildingName;
    [SerializeField]
    private LocalizationText currentValueText;
    [SerializeField]
    private LocalizationText nextValueText;
    [SerializeField]
    private AddressableImage currentNeedItemImage;

    [SerializeField]
    private Image buttonImage;

    [SerializeField]
    private Color activeColor = new Color(1f, 0.84f, 0f);
    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private float nextValue;
    [SerializeField]
    private int currentLevel;
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
    private BigNumber needItemCount;
    [SerializeField]
    private bool isLocked = true;


    [SerializeField]
    private AddressableImage buildingImage;

    [SerializeField]
    private LocalizationText currentLevelText;

    [SerializeField]
    private BuildingTable.BuildingType currentType;

    private List<BuildingTable.Data> datas;

    private BuildingTable.Data currentData;



    private StageManager stageManager;
    [SerializeField]
    private LongPressButton longpressButton;
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        upgradeButton.onClick.AddListener(() => OnClickUpgradeButton());
    }
    private void Start()
    {
        longpressButton.OnLongPress.AddListener(() => OnClickUpgradeButton());
    }

    public void Init(List<BuildingTable.Data> data, int level)
    {
        this.datas = data;
        SetLevelData(level);
    }

    private void SetLevelData(int level)
    {
        currentData = datas[level];
        currentLevel = currentData.Level;
        maxLevel = currentData.MaxLevel;
        needItemCount = currentData.NeedItemCount;

        if (IsMaxLevel())
        {
            SetMaxLevel(currentData);
            return;
        }

        SetNextLevel(level);
        SetLockedImage(level);
        SetBuildingInfo(currentData, currentLevel);
        SetButtonState();
    }
   

    private void SetLockedImage(int level)
    {
        bool isLocked = level <= 0;
        lockedImage.gameObject.SetActive(isLocked);

        var color = buildingImage.GetComponent<Image>().color;
        color.a = isLocked ? 0.2f : 1f;
        buildingImage.GetComponent<Image>().color = color;
    }

    private void SetMaxLevel(BuildingTable.Data buildingData)
    {
        upgradeButton.interactable = false;
        SetBuildingInfo(buildingData, currentLevel);
        UpdateButtonState(false);
    }

    private void SetNextLevel(int level)
    {
        nextLevel = level + 1;
    }
    private void SetLevelText(LocalizationText text, int level)
    {
        text.SetStringArguments(level.ToString());
    }

    private void SetBuildingInfo(BuildingTable.Data buildingData, int level)
    {
        SetBuildingName(buildingData.Type, level);
        SetBuildingText(level);
        SetBuildingExplanationText(buildingData.Type, level);
        SetCurrentNeedImage(buildingData.Type, level);
        SetBuildingImage(buildingData.Type, level);
        SetCountText(buildingData.Type, level);
        SetLevelText(currentLevelText,level);
    }

    private void SetBuildingName(BuildingTable.BuildingType type , int level)
    {
       var data =  DataTableManager.BuildingTable.GetDatas(type);
        if (level >= 0 && level < data.Count)
        {
            var nameId = data[level].NameStringID;
            buildingName.SetString(nameId);
        }
    }

    private void SetBuildingText(int level)
    {
        bool isLocked = level <= 0;
        if (isLocked)
        {
            buildingText.SetString(Defines.ConstructStringId);
        }
        else
        {
            buildingText.SetString(Defines.UpgradeStringId);
        }
        // 테이블 추가 필요
    }

    private void SetBuildingExplanationText(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);

        if (level < 0 || level >= data.Count)
            return;

        var currentData = data[level];
        currentValueText.SetString(currentData.DetailStringID, currentData.Value.ToString("P2"));

        if (level + 1 < data.Count)
        {
            var nextData = data[level + 1];
            nextValueText.SetString(nextData.DetailStringID, nextData.Value.ToString("P2"));
        }
        else
        {
            nextValueText.SetString(Defines.MaxLevelStringId);
        }
    }

    private void SetCurrentNeedImage(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        if(level >= 0 && level< data.Count)
        {
            var spriteId = data[level].NeedItemID;
            currentNeedItemImage.SetItemSprite(spriteId);
        }
    }

    private void SetBuildingImage(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        if (level >= 0 && level < data.Count)
        {
            var BuildingspriteId = data[level].SpriteID;
            buildingImage.SetSprite(BuildingspriteId);
        }
    }

    
    private void SetCountText(BuildingTable.BuildingType type , int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        needItemCount = data[level].NeedItemCount;
        upgradeButtonCountText.text = currentLevel < maxLevel ? $"{needItemCount}" : "최대레벨";
    }

    private void SetButtonState()
    {
        UpdateButtonState(ItemManager.CanConsume(datas[currentLevel].NeedItemID, needItemCount));
    }

    private void UpdateButtonState(bool canUpgrade)
    {
        upgradeButton.interactable = canUpgrade;
        buttonImage.color = canUpgrade ? activeColor : defaultColor;
    }

    public void LevelUp()
    {
        if (currentLevel >= maxLevel) return;

        currentLevel++;
        SetLevelData(currentLevel);
        SaveLoadManager.Data.buildingData.buildingLevels[datas[currentLevel].Type] = currentLevel;
        stageManager.UnitPartyManager.AddBuildingStats(datas[currentLevel].Type, datas[currentLevel].Value);

        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.Building);
        SaveLoadManager.SaveGame();
    }

    private void OnClickUpgradeButton()
    {
        if (IsMaxLevel()) 
            return;

        if (!ItemManager.ConsumeItem(datas[currentLevel].NeedItemID, needItemCount)) 
            return;

        isLocked = false;
        LevelUp();
    }

    private bool IsMaxLevel()
    {
        return currentLevel >= maxLevel;
    }

    private void Update()
    {
        if (IsMaxLevel())
        {
            upgradeButton.interactable = false;
            buttonImage.color = defaultColor;
        }
        else
        {
            SetButtonState();
        }
    }
}
