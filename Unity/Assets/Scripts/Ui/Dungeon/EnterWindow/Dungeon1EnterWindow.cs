using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon1EnterWindow : MonoBehaviour
{
    private StageManager stageManager;

    private List<DungeonTable.Data> subStages;

    private int index;

    [SerializeField]
    private LocalizationText nameText;

    [SerializeField]
    private TextMeshProUGUI selectedDifficulty;

    [SerializeField]
    private LocalizationText conditionStageText;

    [SerializeField]
    private LocalizationText conditionPowerText;

    [SerializeField]
    private AddressableImage needKeyIcon;

    [SerializeField]
    private LocalizationText keyText;

    [SerializeField]
    private AddressableImage clearRewardIcon;

    [SerializeField]
    private TextMeshProUGUI clearRewardText;

    [SerializeField]
    private Button nextDifficultyButton;

    [SerializeField]
    private Button previousDifficultyButton;

    [SerializeField]
    private Button exterminateButton;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    private int maxStage;

    private bool disabled;

    private bool opened;


    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        stageManager.OnIngameStatusChanged += OnIngameStatusChanged;
        ItemManager.OnItemAmountChanged += OnItemAmountChanged;
    }

    public void ShowPopup()
    {
        subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);

        if (!SaveLoadManager.Data.stageSaveData.highestDungeon.ContainsKey(Variables.currentDungeonType))
        {
            SaveLoadManager.Data.stageSaveData.highestDungeon.Add(Variables.currentDungeonType, 1);
        }
        if (!SaveLoadManager.Data.stageSaveData.clearedDungeon.ContainsKey(Variables.currentDungeonType))
        {
            SaveLoadManager.Data.stageSaveData.clearedDungeon.Add(Variables.currentDungeonType, 0);
        }

        maxStage = SaveLoadManager.Data.stageSaveData.highestDungeon[Variables.currentDungeonType];
        SetIndex(maxStage - 1);

        bool clearedAny = SaveLoadManager.Data.stageSaveData.clearedDungeon[Variables.currentDungeonType] > 0;
        exterminateButton.interactable = clearedAny;
    }

    private void OnDisable()
    {
        disabled = true;
    }

    private void OnEnable()
    {
        if (disabled)
        {
            disabled = false;
            ShowPopup();
        }
    }


    private void ShowData(int index)
    {
        var curStage = subStages[index];

        nameText.SetString(curStage.NameStringID);

        selectedDifficulty.text = curStage.Stage.ToString();
        needKeyIcon.SetItemSprite(curStage.NeedKeyItemID);
        keyText.SetStringArguments(curStage.NeedKeyItemCount.ToString(), ItemManager.GetItemAmount(curStage.NeedKeyItemID).ToString());
        conditionPowerText.SetStringArguments(new BigNumber(curStage.NeedPower).ToString());

        int highplanet = SaveLoadManager.Data.stageSaveData.highPlanet;
        if (SaveLoadManager.Data.stageSaveData.highPlanet != SaveLoadManager.Data.stageSaveData.clearedPlanet
             || SaveLoadManager.Data.stageSaveData.highStage != SaveLoadManager.Data.stageSaveData.clearedStage)
        {
            --highplanet;
        }

        conditionStageText.SetStringArguments(subStages[index].NeedClearPlanet.ToString());
        clearRewardIcon.SetItemSprite(curStage.RewardItemID);
        clearRewardText.text = curStage.ClearRewardItemCount.ToString();


        previousDifficultyButton.interactable = index > 0;
        nextDifficultyButton.interactable = index + 1 < maxStage && index < subStages.Count - 1;
    }

    private void OnItemAmountChanged(int itemId, BigNumber amount)
    {
        if (disabled || subStages is null)
        {
            return;
        }
        if (subStages[index].NeedKeyItemID != itemId)
        {
            return;
        }
        keyText.SetStringArguments(subStages[index].NeedKeyItemCount.ToString(), amount.ToString());
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
        if (ItemManager.GetItemAmount(subStages[index].NeedKeyItemID) < subStages[index].NeedKeyItemCount)
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
            return;
        }

        if ((SaveLoadManager.Data.stageSaveData.highPlanet < subStages[index].NeedClearPlanet)
           || (SaveLoadManager.Data.stageSaveData.highPlanet == subStages[index].NeedClearPlanet
               && SaveLoadManager.Data.stageSaveData.highStage != SaveLoadManager.Data.stageSaveData.clearedStage))
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.StageClear);
            return;
        }

        if (UnitCombatPowerCalculator.TotalCombatPower < subStages[index].NeedPower)
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.Power);
            return;
        }

        Variables.currentDungeonStage = index + 1;
        stageManager.SetStatus(IngameStatus.Dungeon);
        opened = true;
        gameObject.SetActive(false);
    }


    private void OnIngameStatusChanged(IngameStatus status)
    {
        if (status != IngameStatus.Planet)
        {
            return;
        }
        if (!opened)
        {
            return;
        }
        opened = false;
        gameObject.SetActive(true);
    }


    public void MoveToShop()
    {
        gameObject.SetActive(false);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetTabActive(3);
    }
}
