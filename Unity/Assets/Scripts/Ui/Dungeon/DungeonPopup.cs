using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
    private TextMeshProUGUI clearRewardText;

    [SerializeField]
    private Button nextDifficultyButton;

    [SerializeField]
    private Button previousDifficultyButton;

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
        selectedDifficulty.text = subStages[index].Stage.ToString();

        previousDifficultyButton.interactable = index > 0;
        nextDifficultyButton.interactable = index + 1 < maxStage && index < subStages.Count - 1;
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
}
