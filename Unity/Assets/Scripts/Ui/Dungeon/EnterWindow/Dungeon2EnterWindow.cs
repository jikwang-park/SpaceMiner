using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon2EnterWindow : MonoBehaviour
{
    private StageManager stageManager;

    private DungeonTable.Data dungeonData;

    [SerializeField]
    private LocalizationText nameText;

    [SerializeField]
    private AddressableImage needKeyIcon;

    [SerializeField]
    private LocalizationText keyText;

    [SerializeField]
    private AddressableImage[] rewardIcons;

    [SerializeField]
    private Button exterminateButton;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

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
        var subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
        dungeonData = subStages[0];

        if (!SaveLoadManager.Data.stageSaveData.highestDungeon.ContainsKey(Variables.currentDungeonType))
        {
            SaveLoadManager.Data.stageSaveData.highestDungeon.Add(Variables.currentDungeonType, 1);
        }
        if (!SaveLoadManager.Data.stageSaveData.clearedDungeon.ContainsKey(Variables.currentDungeonType))
        {
            SaveLoadManager.Data.stageSaveData.clearedDungeon.Add(Variables.currentDungeonType, 0);
        }

        ShowData();

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


    private void ShowData()
    {
        nameText.SetString(dungeonData.NameStringID);

        needKeyIcon.SetItemSprite(dungeonData.NeedKeyItemID);
        keyText.SetStringArguments(dungeonData.NeedKeyItemCount.ToString(), ItemManager.GetItemAmount(dungeonData.NeedKeyItemID).ToString());

        int highplanet = SaveLoadManager.Data.stageSaveData.highPlanet;
        if (SaveLoadManager.Data.stageSaveData.highPlanet != SaveLoadManager.Data.stageSaveData.clearedPlanet
             || SaveLoadManager.Data.stageSaveData.highStage != SaveLoadManager.Data.stageSaveData.clearedStage)
        {
            --highplanet;
        }

        var totalReward = DataTableManager.DamageDungeonRewardTable.GetRewards();
        int iconIndex = 0;

        foreach (var reward in totalReward)
        {
            rewardIcons[iconIndex].transform.parent.gameObject.SetActive(true);
            rewardIcons[iconIndex].SetItemSprite(reward.Key);
            ++iconIndex;
            if (iconIndex == rewardIcons.Length)
            {
                break;
            }
        }

        for (int i = iconIndex; i < rewardIcons.Length; ++i)
        {
            rewardIcons[i].transform.parent.gameObject.SetActive(false);
        }
    }

    //TODO: 인스펙터에서 엔터 버튼과 연결
    public void OnClickEnter()
    {
        if (ItemManager.GetItemAmount(dungeonData.NeedKeyItemID) < dungeonData.NeedKeyItemCount)
        {
            requirementWindow.OpenNeedKey();
            return;
        }

        Variables.currentDungeonStage = dungeonData.Stage;
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

    private void OnItemAmountChanged(int itemId, BigNumber amount)
    {
        if (disabled || dungeonData is null)
        {
            return;
        }
        if (dungeonData.NeedKeyItemID != itemId)
        {
            return;
        }
        keyText.SetStringArguments(dungeonData.NeedKeyItemCount.ToString(), amount.ToString());
    }

    public void MoveToShop()
    {
        gameObject.SetActive(false);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetTabActive(3);
    }
}
