using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuideQuestManager
{
    static GuideQuestManager()
    {
        int questid = SaveLoadManager.Data.questProgressData.currentQuest;
        isCleared = false;
        currentQuestData = DataTableManager.GuideQuestTable.GetDataByOrder(questid);
    }

    public static GuideQuestTable.Data currentQuestData { get; private set; }

    public static bool isCleared;

    public static event System.Action OnClear;
    public static event System.Action OnQuestProgressChanged;

    public static BigNumber Progress;

    public static void QuestProgressChange(GuideQuestTable.MissionType type)
    {
        if (currentQuestData is null)
        {
            return;
        }

        if (currentQuestData.MissionClearType != type)
        {
            return;
        }

        bool isClearedNow = false;

        switch (type)
        {
            case GuideQuestTable.MissionType.Exterminate:
                Progress = SaveLoadManager.Data.questProgressData.monsterCount;
                isClearedNow = currentQuestData.TargetCount <= SaveLoadManager.Data.questProgressData.monsterCount;
                break;

            case GuideQuestTable.MissionType.StageClear:
                var stageData = DataTableManager.StageTable.GetData(currentQuestData.Target);
                var stageSaveData = SaveLoadManager.Data.stageSaveData;

                Progress = DataTableManager.StageTable.GetStageData(stageSaveData.clearedPlanet, stageSaveData.clearedStage).ID;

                isClearedNow = (stageSaveData.clearedPlanet == stageData.Planet && stageSaveData.clearedStage >= stageData.Stage)
                    || (stageSaveData.clearedPlanet > stageData.Planet);
                break;

            case GuideQuestTable.MissionType.DungeonClear:
                var dungeonData = DataTableManager.DungeonTable.GetData(currentQuestData.Target);
                var clearedDungeon = SaveLoadManager.Data.stageSaveData.clearedDungeon;
                Progress = clearedDungeon[dungeonData.Type];
                isClearedNow = clearedDungeon[dungeonData.Type] >= dungeonData.Stage;
                break;

            case GuideQuestTable.MissionType.StatUpgrade:
                var statData = SaveLoadManager.Data.unitStatUpgradeData;
                var upgradeData = DataTableManager.UnitUpgradeTable.GetData(currentQuestData.Target);
                Progress = statData.upgradeLevels[upgradeData.Type];
                isClearedNow = statData.upgradeLevels[upgradeData.Type] >= currentQuestData.TargetCount;
                break;

            case GuideQuestTable.MissionType.Item:
                if (!isCleared)
                {
                    Progress = ItemManager.GetItemAmount(currentQuestData.Target);
                }
                isClearedNow = Progress >= currentQuestData.TargetCount;
                break;

            case GuideQuestTable.MissionType.Building:
                var buildingData = DataTableManager.BuildingTable.GetData(currentQuestData.Target);

                Progress = SaveLoadManager.Data.buildingData.buildingLevels[buildingData.Type];
                isClearedNow = Progress >= currentQuestData.TargetCount;
                break;
        }

        OnQuestProgressChanged?.Invoke();

        if (isClearedNow)
        {
            isCleared = true;
            OnClear.Invoke();
        }
    }

    public static void GetReward()
    {
        ItemManager.AddItem(currentQuestData.RewardItemID, currentQuestData.RewardItemCount);
        ChangeQuest(currentQuestData.Turn + 1);
        SaveLoadManager.SaveGame();
    }

    public static void RefreshQuest()
    {
        QuestProgressChange(currentQuestData.MissionClearType);
    }

    public static void ChangeQuest(int turn)
    {
        isCleared = false;
        SaveLoadManager.Data.questProgressData.currentQuest = turn;
        currentQuestData = DataTableManager.GuideQuestTable.GetDataByOrder(turn);
        RefreshQuest();
    }
}
