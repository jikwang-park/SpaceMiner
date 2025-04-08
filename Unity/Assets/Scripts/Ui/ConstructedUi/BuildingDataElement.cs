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
    private TextMeshProUGUI upgradeButtonText;
    [SerializeField]
    private Image constructionImage;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI constructionExplanText;


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
        level = 0;
        SetLevelData(level);
        isLocked = true;
    }

    public void SetLevelData(int level)
    {
        id = data[level].ID;
        this.level = data[level].Level;
        value = data[level].Value;
        itemId = data[level].ItemID;
        maxLevel = data[level].MaxLevel;
        uisequence = data[level].Sequence;
        needItemCount = data[level].NeedCount;
    }

    public void GetCurrentSequence()
    {

    }

    private void SetConstructionInfo()
    {
        SetLevelText(level);
        SetConstructionExplanText(currentType);
    }

    private void SetLevelText(int level)
    {
        levelText.text = $"LV.{level}";
    }
    private void SetConstructionExplanText(BuildingTable.BuildingType type)
    {
        constructionExplanText.text = $"{type.ToString()} \n {itemId} �� {needItemCount} �� �ʿ��մϴ�";
    }
    public void SetData()
    {
        //���߿� ���̺� �����Ϳ��� ����
    }

    public float GetCurrentValue()
    {
        return 0;
    }

    public void LevelUp()
    {
        level++;
        SetLevelData(level);
        

    }

    private void OnClickUpgradeButton()
    {
        if(isLocked)
        {
            isLocked = false;

        }
    }
}
