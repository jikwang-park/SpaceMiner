using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideQuestWindow : MonoBehaviour
{
    [SerializeField]
    private LocalizationText questDetailText;

    [SerializeField]
    private TextMeshProUGUI questProgressText;

    [SerializeField]
    private TextMeshProUGUI rewardCountText;

    [SerializeField]
    private AddressableImage questRewardIcon;

    private bool cleared;

    private StageManager stageManager;

    private void Awake()
    {
        cleared = false;
        GuideQuestManager.OnQuestProgressChanged += UpdateProgress;
        GuideQuestManager.OnClear += OnQuestClear;
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        if (GuideQuestManager.currentQuestData is not null)
        {
            SetQuestTargetReward();
            var itemSprite = DataTableManager.ItemTable.GetData(GuideQuestManager.currentQuestData.RewardItemID);
            questRewardIcon.SetSprite(itemSprite.SpriteID);
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

        int monsterCount = SaveLoadManager.Data.questProgressData.monsterCount;
        int goal = GuideQuestManager.currentQuestData.TargetCount;

        questProgressText.text = $"{GuideQuestManager.Progress} / {goal}";
    }

    private void OnQuestClear()
    {
        cleared = true;
        questProgressText.text = $"{questProgressText.text} Clear";
    }

    private void SetQuestTargetReward()
    {
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
                questDetailText.SetString(questData.DetailStringID, dungeonData.NameStringID);
                break;
            case GuideQuestTable.MissionType.StatUpgrade:
                var upgradeData = DataTableManager.UnitUpgradeTable.GetData(questData.Target);
                questDetailText.SetString(questData.DetailStringID, upgradeData.Type.ToString(), questData.TargetCount.ToString());
                break;
            case GuideQuestTable.MissionType.Item:
                var itemData = DataTableManager.ItemTable.GetData(questData.Target);
                var itemName = DataTableManager.StringTable.GetData(itemData.NameStringID);
                questDetailText.SetString(questData.DetailStringID, itemName, questData.TargetCount.ToString());
                break;
            case GuideQuestTable.MissionType.Building:
                var buildingData = DataTableManager.BuildingTable.GetData(questData.Target);
                questDetailText.SetString(questData.DetailStringID, buildingData.NameStringID);
                break;
        }
        var itemSprite = DataTableManager.ItemTable.GetData(questData.RewardItemID);
        questRewardIcon.SetSprite(itemSprite.SpriteID);
        int rewardCount = questData.RewardItemCount;
        rewardCountText.text = rewardCount.ToString();
    }

    public void QuestSuccess()
    {
        if (cleared)
        {
            cleared = false;
            stageManager.StageUiManager.IngameUIManager.GuideQuestRewardWindow.Show(GuideQuestManager.currentQuestData);
            GuideQuestManager.GetReward();
            SetQuestTargetReward();
            UpdateProgress();
        }
    }
}
