
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[Serializable]
public class InventoryElementSaveData
{
    public int soldierId; //250331 HKY �������� ����
    public bool isLocked;
    public Grade grade;
    public int count;
    public int level;
}
[Serializable]
public class InventorySaveData
{
    public List<InventoryElementSaveData> elements = new List<InventoryElementSaveData>();
    public int equipElementID; //250331 HKY �������� ����
    public UnitTypes inventoryType;
}
[Serializable]
public class StageSaveData
{
    public int currentPlanet;
    public int currentStage;
    public int highPlanet;
    public int highStage;
}

[Serializable]
public class TotalSaveData
{
    public Dictionary<UnitTypes, InventorySaveData> inventorySaveData = new Dictionary<UnitTypes, InventorySaveData>();
    public StageSaveData stageSaveData;
    public Dictionary<int, BigNumber> itemSaveData = new Dictionary<int, BigNumber>();
}