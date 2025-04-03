using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPopup : MonoBehaviour
{
    private StageManager stageManager;

    private List<DungeonTable.Data> subStages;

    private int index;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI difficultyText;

    [SerializeField]
    private TextMeshProUGUI selectedDifficulty;

    [SerializeField]
    private TextMeshProUGUI conditionStageText;

    [SerializeField]
    private TextMeshProUGUI conditionPowerText;

    [SerializeField]
    private TextMeshProUGUI keyText;

    [SerializeField]
    private TextMeshProUGUI clearRewardText;

    [SerializeField]
    private Button nextDifficultyButton;

    [SerializeField]
    private Button previousDifficultyButton;

    [SerializeField]
    private Button enterButton;

    private int maxStage;


    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void ShowPopup(int dungeonType)
    {
        subStages = DataTableManager.DungeonTable.GetDungeonList(dungeonType);
        Variables.currentDungeonType = dungeonType;
        maxStage = SaveLoadManager.Data.stageSaveData.highestDungeon[dungeonType];
        SetIndex(maxStage - 1);
    }

    private void ShowData(int index)
    {
        var curStage = subStages[index];

        selectedDifficulty.text = $"Stage : {curStage.Stage}";
        keyText.text = $"{curStage.KeyCount} / {ItemManager.GetItemAmount(curStage.DungeonKeyID)}";
        conditionPowerText.text = $"Currrent Power : {Variables.powerLevel}\nNeed : {curStage.ConditionPower}";
        conditionStageText.text = $"Currrent Planet : {SaveLoadManager.Data.stageSaveData.highPlanet - 1}\nNeed : {subStages[index].ConditionPlanet}";
        clearRewardText.text = $"Reward : {curStage.ItemID}/{curStage.ClearReward}";


        previousDifficultyButton.interactable = index > 0;
        nextDifficultyButton.interactable = index + 1 < maxStage && index < subStages.Count - 1;

        bool powerCondition = Variables.powerLevel > curStage.ConditionPower;
        bool planetCondition = SaveLoadManager.Data.stageSaveData.highPlanet > curStage.ConditionPlanet;
        bool keyCondition = ItemManager.GetItemAmount(curStage.DungeonKeyID) >= curStage.KeyCount;

        enterButton.interactable = powerCondition && planetCondition && keyCondition;
    }

    private void SetIndex(int index)
    {
        this.index = index;

        ShowData(this.index);
    }

    //TODO: 인스펙터에서 스테이지 뒤로, 앞으로 버튼과 연결
    public void OnClickStage(bool isNext)
    {
        bool changed = false;

        if (isNext && index + 1 < maxStage && index < subStages.Count - 1)
        {
            ++index;
            changed = true;
        }
        else if (!isNext && index > 0)
        {
            --index;
            changed = true;
        }

        if (changed)
        {
            ShowData(index);
        }
    }

    //TODO: 인스펙터에서 엔터 버튼과 연결
    public void OnClickEnter()
    {
        Variables.currentDungeonStage = index + 1;
        stageManager.SetStatus(IngameStatus.Dungeon);
    }

    public void OnClickKeyGet()
    {
        ItemManager.AddItem(subStages[index].DungeonKeyID, 1);
        ShowData(index);
    }
}
