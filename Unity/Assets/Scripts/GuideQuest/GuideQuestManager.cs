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
        if (currentQuestData.MissionClearType != type)
        {
            return;
        }
        if (isCleared)
        {
            return;
        }

        OnQuestProgressChanged?.Invoke();
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

                isClearedNow = (stageSaveData.clearedPlanet == stageData.Planet && stageSaveData.clearedStage >= stageData.Stage)
                    || (stageSaveData.clearedPlanet > stageData.Planet);

                if (isClearedNow)
                {
                    Progress = 1;
                }
                else
                {
                    Progress = 0;
                }

                break;

            case GuideQuestTable.MissionType.DungeonClear:
                var dungeonData = DataTableManager.DungeonTable.GetData(currentQuestData.Target);
                var clearedDungeon = SaveLoadManager.Data.stageSaveData.clearedDungeon;
                isClearedNow = clearedDungeon[dungeonData.Type] >= dungeonData.Stage;

                if (isClearedNow)
                {
                    Progress = 1;
                }
                else
                {
                    Progress = 0;
                }
                break;

            case GuideQuestTable.MissionType.StatUpgrade:
                var statData = SaveLoadManager.Data.unitStatUpgradeData;
                isClearedNow = statData.upgradeLevels[(UnitUpgradeTable.UpgradeType)currentQuestData.Target] >= currentQuestData.TargetCount;

                if (isClearedNow)
                {
                    Progress = 1;
                }
                else
                {
                    Progress = 0;
                }

                break;

            case GuideQuestTable.MissionType.Item:
                Progress = ItemManager.GetItemAmount(currentQuestData.Target);
                isClearedNow = Progress >= currentQuestData.TargetCount;
                break;

            case GuideQuestTable.MissionType.Building:
                Progress = SaveLoadManager.Data.buildingData.buildingLevels[(BuildingTable.BuildingType)currentQuestData.Target];
                isClearedNow = Progress >= currentQuestData.TargetCount;
                break;
        }

        if (isClearedNow)
        {
            isCleared = true;
            OnClear.Invoke();
        }
    }

    public static void GetReward()
    {
        isCleared = false;
        ItemManager.AddItem(currentQuestData.RewardID, currentQuestData.RewardCount);
        ++SaveLoadManager.Data.questProgressData.currentQuest;
        int questid = SaveLoadManager.Data.questProgressData.currentQuest;
        currentQuestData = DataTableManager.GuideQuestTable.GetDataByOrder(questid);
        QuestProgressChange(currentQuestData.MissionClearType);
        SaveLoadManager.SaveGame();
    }
}
