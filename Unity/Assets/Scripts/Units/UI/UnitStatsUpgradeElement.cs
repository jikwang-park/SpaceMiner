using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitUpgradeTable;

public class UnitStatsUpgradeElement : MonoBehaviour
{
    public UpgradeType currentType;

    public float value = 0;

    public BigNumber gold = 0;

    public int maxLevel = 0;
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


    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        addStartButtonText.text = $"+ {gold} ";
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());
        levelText.text = $"{level}";
    }


    public void Init(UnitUpgradeTable.Data data)
    {
        currentType = data.Type;
        level = 0;
        value = data.Value;
        gold = data.Gold;
        maxLevel = data.MaxLevel;
    }

    private void SetStatsInfo()
    {
        nextLevel = level + 1;
        statsInformation.text = $"Unit {currentType.ToString()} Increase\n +{value + (value* currentNum)}";
        addStartButtonText.text = $"Gold \n +{gold * nextLevel}";
        LevelUp();
    }
  

    private void LevelUp()
    {
        if (level > 1000)
        {
            return;
        }

        level++;
        currentNum++;
    }
    private void OnClickAddStatsButton()
    {
        SetStatsInfo();
        stageManager.UnitPartyManager.AddStats(currentType, value + (value * currentNum));
    }

 
}
