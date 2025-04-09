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
    public int level = 0;

    public int nextLevel = 0;

    [SerializeField]
    private Button addStatButton;
    [SerializeField]
    private TextMeshProUGUI addStartButtonText;
    [SerializeField]
    private TextMeshProUGUI statsInformation;
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

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>(); 
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());
    }


    public void Init(UnitUpgradeTable.Data data)
    {
        currentType = data.Type;
        level = 0;
        value = data.Value;
        gold = data.Gold;
        maxLevel = data.MaxLevel;
        SetStatsInfo();
    }

    private void SetStatsInfo()
    {
        nextLevel = level + 1;
        levelText.text = $"Level + {level}";
        statsInformation.text = $"Unit {currentType.ToString()} Increase\n +{currentValue:F2} -> {currentValue + value:F2}";
        addStartButtonText.text = $"Gold \n +{currentGold +gold }";
    }
    
    public void SetData(int level)
    {
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
        for(int i = 1; i<=level; ++i)
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
        
        currentValue +=  value;
        currentGold += gold * (level + 1);
        level++;
        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.StatUpgrade);
        SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels[currentType] = level;
    }
    private void OnClickAddStatsButton()
    {
        LevelUp();
        SetStatsInfo();
        stageManager.UnitPartyManager.AddStats(currentType, level*value);
        SaveLoadManager.SaveGame();
    }

     
 
}
