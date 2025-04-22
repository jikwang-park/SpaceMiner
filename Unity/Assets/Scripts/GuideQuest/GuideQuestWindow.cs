using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideQuestWindow : MonoBehaviour
{
    [SerializeField]
    private Color clearedBackColor = new Color(0.5f, 1f, 0.5f, 0.4f);
    [SerializeField]
    private Color inProgressBackColor = new Color(0f, 0f, 0f, 0.4f);


    [SerializeField]
    private LocalizationText questDetailText;

    [SerializeField]
    private TextMeshProUGUI questProgressText;

    [SerializeField]
    private TextMeshProUGUI rewardCountText;

    [SerializeField]
    private AddressableImage questRewardIcon;

    private Image background;

    private StageManager stageManager;

    private void Awake()
    {
        background = GetComponent<Image>();
        GuideQuestManager.OnQuestChanged += SetQuestTargetReward;
        GuideQuestManager.OnQuestProgressChanged += UpdateProgress;
        GuideQuestManager.OnClear += OnQuestClear;
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        if (GuideQuestManager.currentQuestData is not null)
        {
            SetQuestTargetReward();
            questRewardIcon.SetItemSprite(GuideQuestManager.currentQuestData.RewardItemID);
            UpdateProgress();
            GuideQuestManager.RefreshQuest();
        }
    }

    private void UpdateProgress()
    {
        if (GuideQuestManager.currentQuestData is null)
        {
            return;
        }
        var questData = GuideQuestManager.currentQuestData;
        switch (questData.MissionClearType)
        {
            case GuideQuestTable.MissionType.Exterminate:
            case GuideQuestTable.MissionType.Item:
                questProgressText.text = $"{GuideQuestManager.Progress} / {questData.TargetCount}";
                break;
            case GuideQuestTable.MissionType.StageClear:
            case GuideQuestTable.MissionType.DungeonClear:
            case GuideQuestTable.MissionType.StatUpgrade:
            case GuideQuestTable.MissionType.Building:
                questProgressText.text = $"{GuideQuestManager.Progress} / 1";
                break;
        }
    }

    private void OnQuestClear()
    {
        background.color = clearedBackColor;
    }

    private void SetQuestTargetReward()
    {
        background.color = inProgressBackColor;
        var questData = GuideQuestManager.currentQuestData;

        switch (questData.MissionClearType)
        {
            case GuideQuestTable.MissionType.Exterminate:
                questDetailText.SetString(questData.DetailStringID);
                break;
            case GuideQuestTable.MissionType.StageClear:
                var stageData = DataTableManager.StageTable.GetData(questData.Target);
                questDetailText.SetString(questData.DetailStringID, stageData.Planet.ToString(), stageData.Stage.ToString());
                break;
            case GuideQuestTable.MissionType.DungeonClear:
                var dungeonData = DataTableManager.DungeonTable.GetData(questData.Target);
                questDetailText.SetString(questData.DetailStringID, dungeonData.Stage.ToString());
                break;
            case GuideQuestTable.MissionType.StatUpgrade:
                var upgradeData = DataTableManager.UnitUpgradeTable.GetData(questData.Target);
                var upgradeName = DataTableManager.StringTable.GetData(upgradeData.NameStringID);
                questDetailText.SetString(questData.DetailStringID, upgradeName, questData.TargetCount.ToString());
                break;
            case GuideQuestTable.MissionType.Item:
                var itemData = DataTableManager.ItemTable.GetData(questData.Target);
                var itemName = DataTableManager.StringTable.GetData(itemData.NameStringID);
                questDetailText.SetString(questData.DetailStringID, itemName, questData.TargetCount.ToString());
                break;
            case GuideQuestTable.MissionType.Building:
                var buildingData = DataTableManager.BuildingTable.GetData(questData.Target);
                questDetailText.SetString(questData.DetailStringID, buildingData.Level.ToString());
                break;
        }
        questRewardIcon.SetItemSprite(questData.RewardItemID);
        BigNumber rewardCount = questData.RewardItemCount;
        rewardCountText.text = rewardCount.ToString();
    }

    public void QuestSuccess()
    {
        if (!GuideQuestManager.isCleared)
        {
            return;
        }
        stageManager.StageUiManager.IngameUIManager.GuideQuestRewardWindow.Show(GuideQuestManager.currentQuestData);
        GuideQuestManager.GetReward();
        SetQuestTargetReward();
        UpdateProgress();
        if (GuideQuestManager.isCleared)
        {
            background.color = clearedBackColor;
        }
    }
}
