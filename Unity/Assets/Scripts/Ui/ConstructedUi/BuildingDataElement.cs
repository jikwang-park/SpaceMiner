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
    private int needItemCount;
    [SerializeField]
    private bool isLocked = true;

    private const int maxlevelText = 60010;

    [SerializeField]
    private AddressableImage buildingImage;
    

    [SerializeField]
    private BuildingTable.BuildingType currentType;

    private List<BuildingTable.Data> data;

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
        this.data = data;
        SetLevelData(level);
    }

    private void SetLevelData(int level)
    {
        var buildingData = data[level];
        currentLevel = buildingData.Level;
        maxLevel = buildingData.MaxLevel;
        needItemCount = buildingData.NeedItemCount;

        if (IsMaxLevel())
        {
            SetMaxLevel(buildingData);
            return;
        }

        SetNextLevel(level);
        SetBuildingInfo(buildingData, level);
        SetButtonState();
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

    private void SetBuildingInfo(BuildingTable.Data buildingData, int level)
    {
        SetBuildingName(buildingData, level);
        SetBuildingText();
        SetBuildingExplanationText(buildingData, level);
        SetCurrentNeedImage(buildingData, level);
        SetBuildingImage(buildingData, level);
        SetCountText(buildingData);
        SetLevelText(level);
    }

    private void SetBuildingName(BuildingTable.Data buildingData, int level)
    {
        var nameId = buildingData.NameStringID;
        buildingName.SetString(nameId);
    }

    private void SetBuildingText()
    {
        buildingText.SetString(90); // 테이블 추가 필요
    }

    private void SetBuildingExplanationText(BuildingTable.Data buildingData, int level)
    {
        int explanationText = buildingData.DetailStringID;
        currentValueText.SetString(explanationText, buildingData.Value.ToString("P2"));

        if (level < maxLevel - 1)
        {
            var nextBuildingData = data[level + 1];
            int nextExplanationText = nextBuildingData.DetailStringID;
            nextValueText.SetString(nextExplanationText, nextBuildingData.Value.ToString("P2"));
        }
        else
        {
            nextValueText.SetString(maxlevelText);
        }
    }

    private void SetCurrentNeedImage(BuildingTable.Data buildingData, int level)
    {
        var spriteId = buildingData.NeedItemID;
        currentNeedItemImage.SetSprite(spriteId);
    }

    private void SetBuildingImage(BuildingTable.Data buildingData, int level)
    {
        var spriteId = buildingData.SpriteID;
        buildingImage.SetSprite(spriteId);
    }

    private void SetLevelText(int level)
    {
        levelText.text = $"LV.{level}";
    }

    private void SetCountText(BuildingTable.Data buildingData)
    {
        upgradeButtonCountText.text = currentLevel < maxLevel ? $"{needItemCount}" : "최대레벨";
    }

    private void SetButtonState()
    {
        UpdateButtonState(ItemManager.CanConsume(data[currentLevel].NeedItemID, needItemCount));
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
        stageManager.UnitPartyManager.AddBuildingStats(data[currentLevel].Type, data[currentLevel].Value);

        SaveLoadManager.Data.buildingData.buildingLevels[data[currentLevel].Type] = currentLevel;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.Building);
        SaveLoadManager.SaveGame();
    }

    private void OnClickUpgradeButton()
    {
        if (IsMaxLevel()) 
            return;

        if (!ItemManager.ConsumeItem(data[currentLevel].NeedItemID, needItemCount)) 
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
