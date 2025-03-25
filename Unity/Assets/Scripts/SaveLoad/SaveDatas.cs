
using System.Collections.Generic;
using System;

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
    public string currentElementID;
    public UnitTypes inventoryType;
}
[Serializable]
public class TotalSaveData
{
    public Dictionary<UnitTypes, InventorySaveData> inventorySaveData = new Dictionary<UnitTypes, InventorySaveData>();
}