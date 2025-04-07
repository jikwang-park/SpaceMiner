
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
    public Dictionary<int, int> highestDungeon = new Dictionary<int, int>();
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

    public MiningRobotInventoryData(int totalSlots = 60)
    {
        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new MiningRobotInventorySlotData());
        }
    }
}

