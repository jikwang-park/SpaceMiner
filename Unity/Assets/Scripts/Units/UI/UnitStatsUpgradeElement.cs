using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnitUpgradeTable;

public class UnitStatsUpgradeElement : MonoBehaviour
{
    public UpgradeType currentType;

    public BigNumber value;

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


    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        addStartButtonText.text = $"+ {gold} ";
        addStatButton.onClick.AddListener(() => OnClickAddStatsButton());
        levelText.text = $"{level}";
    }


    private void Init()
    {
        
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

    public UnitUpgradeTable.Data SetData(int id)
    {
        return DataTableManager.UnitUpgradeTable.GetData(id);
    }
}
