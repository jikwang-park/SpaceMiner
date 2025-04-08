
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[Serializable]
public class SoldierInventoryElementData
{
    public int soldierId; //250331 HKY 데이터형 변경
    public bool isLocked;
    public Grade grade;
    public int count;
    public int level;
}
[Serializable]
public class SoldierInventoryData
{
    public List<SoldierInventoryElementData> elements = new List<SoldierInventoryElementData>();
    public int equipElementID; //250331 HKY 데이터형 변경
    public UnitTypes inventoryType;
}
[Serializable]
public class StageSaveData
{
    public int currentPlanet;
    public int currentStage;
    public int highPlanet;
    public int highStage;
    public int clearedPlanet;
    public int clearedStage;
    public Dictionary<int, int> highestDungeon = new Dictionary<int, int>();
    public Dictionary<int, int> clearedDungeon = new Dictionary<int, int>();
}
[Serializable]
public class MiningRobotInventorySlotData
{
    public bool isEmpty = true;
    public int miningRobotId;
}
[Serializable]
public class MiningRobotInventoryData
{
    public List<MiningRobotInventorySlotData> slots = new List<MiningRobotInventorySlotData>();
    public MiningRobotInventoryData() { }
    public static MiningRobotInventoryData CreateDefault(int totalSlots = 60)
    {
        var data = new MiningRobotInventoryData();
        for (int i = 0; i < totalSlots; i++)
        {
            data.slots.Add(new MiningRobotInventorySlotData());
        }
        return data;
    }
}
[Serializable]
public class UnitStatUpgradeData
{
    public Dictionary<UnitUpgradeTable.UpgradeType, int> upgradeLevels = new Dictionary<UnitUpgradeTable.UpgradeType, int>();
    public UnitStatUpgradeData() { }

    public static UnitStatUpgradeData CreateDefault()
    {
        var data = new UnitStatUpgradeData();
        foreach (var type in Enum.GetValues(typeof(UnitUpgradeTable.UpgradeType)))
        {
            data.upgradeLevels.Add((UnitUpgradeTable.UpgradeType)type, 1);
        }
        return data;
    }
}
[Serializable]
public class UnitSkillUpgradeData
{
    public Dictionary<UnitTypes, Dictionary<Grade, int>> skillUpgradeId = new Dictionary<UnitTypes, Dictionary<Grade, int>>();
    public UnitSkillUpgradeData() { }
    public static UnitSkillUpgradeData CreateDefault()
    {
        var data = new UnitSkillUpgradeData();
        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            Dictionary<Grade, int> gradeDict = new Dictionary<Grade, int>();
            foreach (Grade grade in Enum.GetValues(typeof(Grade)))
            {
                string name = type.ToString() + grade.ToString();
                int defaultId = DataTableManager.DefaultDataTable.GetID(name);
                gradeDict.Add(grade, defaultId);
            }
            data.skillUpgradeId.Add(type, gradeDict);
        }
        return data;
    }
}
[Serializable]
public class QuestProgressData
{
    public int currentQuest;
    public int monsterCount;
    public QuestProgressData() { }
    public static QuestProgressData CreateDefault()
    {
        var data = new QuestProgressData();
        data.currentQuest = 1;
        data.monsterCount = 0;
        return data;
    }
}
[Serializable]
public class BuildingData
{
    public Dictionary<BuildingTable.BuildingType, int> buildingLevels = new Dictionary<BuildingTable.BuildingType, int>();
    public BuildingData() { }
    public static BuildingData CreateDefault()
    {
        var data = new BuildingData();
        foreach(var type in Enum.GetValues(typeof(BuildingTable.BuildingType)))
        {
            data.buildingLevels.Add((BuildingTable.BuildingType)type, 0);
        }
        return data;
    }
}