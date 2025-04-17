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
    public static event System.Action OnQuestChanged;

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
                isClearedNow = new BigNumber(currentQuestData.TargetCount) <= SaveLoadManager.Data.questProgressData.monsterCount;
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
                var upgradeData = DataTableManager.UnitUpgradeTable.GetData(currentQuestData.Target);
                isClearedNow = statData.upgradeLevels[upgradeData.Type] >= new BigNumber(currentQuestData.TargetCount);
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
                if (!isCleared)
                {
                    Progress = ItemManager.GetItemAmount(currentQuestData.Target);
                }
                isClearedNow = Progress >= currentQuestData.TargetCount;
                break;

            case GuideQuestTable.MissionType.Building:
                var buildingData = DataTableManager.BuildingTable.GetData(currentQuestData.Target);

                isClearedNow = SaveLoadManager.Data.buildingData.buildingLevels[buildingData.Type] >= buildingData.Level;

                if (isClearedNow)
                {
                    Progress = 1;
                }
                else
                {
                    Progress = 0;
                }
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
        SaveLoadManager.Data.questProgressData.monsterCount = 0;
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
        OnQuestChanged?.Invoke();
        RefreshQuest();
    }
}
