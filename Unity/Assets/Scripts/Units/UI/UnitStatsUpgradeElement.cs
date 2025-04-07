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
        statsInformation.text = $"Unit {currentType.ToString()} Increase\n +{(value + (value* currentNum)):F2}";
        addStartButtonText.text = $"Gold \n +{gold * nextLevel}";
        LevelUp();
    }
    
    public void SetData(int level)
    {
        //로드한 데이터를 토대로 level로 세팅 해주기
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
