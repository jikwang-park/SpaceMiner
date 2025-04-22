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



    private int statsMultiplier = 1;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());

    }



    public void SetMultiplier(int multiplier)
    {
        statsMultiplier = multiplier;
        SetStatsInfo();
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
        nextLevel = level + statsMultiplier;
        levelText.text = $"Level + {level} -> {nextLevel}";
        float beforeValue = level * value;
        float afterValue = nextLevel * value;

        BigNumber afterGold = GetCurrentGold(nextLevel);

        switch (currentType)
        {
            case UpgradeType.AttackPoint:
            case UpgradeType.HealthPoint:
            case UpgradeType.DefensePoint:
                beforeStatsInfo.text = $"{beforeValue:F2}";
                afterStatsInfo.text = $"{afterValue:F2}";
                break;
            case UpgradeType.CriticalPossibility:
            case UpgradeType.CriticalDamages:
                beforeStatsInfo.text = $"{(beforeValue * 100):F2}%";
                afterStatsInfo.text = $"{((afterValue) * 100):F2}%";
                break;
        }
        BigNumber neededGold = GetGoldForMultipleLevels(level, statsMultiplier);

        addStartButtonText.text = $" +{neededGold}";
    }
    public BigNumber GetGoldForMultipleLevels(int currentLevel, int multiplier)
    {
        int start = currentLevel + 1;
        int end = Mathf.Min(currentLevel + multiplier, maxLevel);

        BigNumber result = 0;
        for (int i = start; i <= end; ++i)
        {
            result += gold * i;
        }
        return result;
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

        int addLevel = statsMultiplier;

        level += addLevel;
        currentValue += level * value;
        currentGold += GetCurrentGold(level);





        SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[currentType] = level;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StatUpgrade);
    }
    private void OnClickAddStatsButton()
    {
        BigNumber totalGold = GetGoldForMultipleLevels(level , statsMultiplier);
        if (ItemManager.CanConsume((int)Currency.Gold, totalGold))
        {
            ItemManager.ConsumeCurrency(Currency.Gold, totalGold);
            LevelUp();
            SetStatsInfo();
            stageManager.UnitPartyManager.AddStats(currentType, value * statsMultiplier);
            SaveLoadManager.SaveGame();
        }
    }
}
