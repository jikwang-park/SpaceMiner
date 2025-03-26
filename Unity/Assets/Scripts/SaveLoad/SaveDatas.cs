
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[Serializable]
public class InventoryElementSaveData
{
    public string soldierId;
    public bool IsLocked;
    public int Count;
    public int Level;
}
[Serializable]
public class InventorySaveData
{
    public List<InventoryElementSaveData> elements = new List<InventoryElementSaveData>();
    public string equipElementID;
    public UnitTypes inventoryType;
}
[Serializable]
public class StageSaveData
{
    public int currentPlanet;
    public int currentStage;
    public int HighPlanet;
    public int HighStage;
}
[Serializable]
public class TotalSaveData
{
    public Dictionary<UnitTypes, InventorySaveData> inventorySaveData = new Dictionary<UnitTypes, InventorySaveData>();
}