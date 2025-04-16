using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnitUpgradeTable;

public class UnitStatsUpgradeElement : MonoBehaviour
{
    public UpgradeType currentType;

    public float value;

    public BigNumber gold;

    public int maxLevel;
    // 아직 테이블에 없음
    public int level;

    public int nextLevel = 0;

    [SerializeField]
    private Button addStatButton;
    [SerializeField]
    private TextMeshProUGUI addStartButtonText;
    [SerializeField]
    private UnitPartyManager unitPartyManager;
    [SerializeField]
    private StageManager stageManager;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private int currentNum = 0;
    [SerializeField]
    private float currentValue = 0;
    [SerializeField]
    private BigNumber currentGold = 0;

    [SerializeField]
    private TextMeshProUGUI beforeStatsInfo;
    [SerializeField]
    private TextMeshProUGUI afterStatsInfo;
    [SerializeField]
    private LocalizationText titleText;
    [SerializeField]
    private Image StatsImage;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());
    }

    public void SetInitString(UpgradeType type)
    {
        var stringId = DataTableManager.UnitUpgradeTable.GetData(type).NameStringID;
        titleText.SetString(stringId);
    }
    //임시 처리
    public void SetImage(UpgradeType type, List<Sprite> statsSprite)
    {
        int index = (int)type;

        StatsImage.sprite = statsSprite[index - 1];
    }

    public void Init(UnitUpgradeTable.Data data)
    {
        currentType = data.Type;
        value = data.Value;
        gold = data.NeedItemCount;
        maxLevel = data.MaxLevel;
        SetStatsInfo();
    }

    private void SetStatsInfo()
    {
        nextLevel = level + 1;
        levelText.text = $"Level + {level}";
        beforeStatsInfo.text = $"{currentValue:F2}";
        afterStatsInfo.text = $"{currentValue + value:F2}";
        addStartButtonText.text = $" +{currentGold + gold}";
    }

    public void SetData(int level, UnitUpgradeTable.Data data)
    {
        currentType = data.Type;
        value = data.Value;
        gold = data.NeedItemCount;
        maxLevel = data.MaxLevel;

        this.level = level;
        currentValue = GetCurrentValue(level);
        currentGold = GetCurrentGold(level);
        SetStatsInfo();
        Debug.Log(this.level);
    }

    public float GetCurrentValue(int level)
    {
        float result = 0;
        result = value * level;
        return result;
    }
    public BigNumber GetCurrentGold(int level)
    {
        BigNumber result = 0;
        for (int i = 1; i <= level; ++i)
        {
            result += gold * i;
        }
        return result;
    }

    private void LevelUp()
    {

        if (level > 1000)
        {
            addStatButton.interactable = false;
        }

        level++;


        currentValue += value;
        currentGold += gold * (level + 1);
        SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[currentType] = level;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StatUpgrade);
    }
    private void OnClickAddStatsButton()
    {
        if (ItemManager.CanConsume((int)Currency.Gold, currentGold))
        {
            ItemManager.ConsumeCurrency(Currency.Gold, currentGold);
            LevelUp();
            SetStatsInfo();
            stageManager.UnitPartyManager.AddStats(currentType, level * value);
            SaveLoadManager.SaveGame();
        }
    }
}
