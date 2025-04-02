using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryManager
{
    private static Dictionary<UnitTypes, InventorySaveData> inventories = new Dictionary<UnitTypes, InventorySaveData>();
    public static readonly int requireMergeCount = 5;
    public static event Action onChangedInventory;
    static InventoryManager()
    {
        inventories.Clear();
        Initialize();
        SaveLoadManager.onSaveRequested += DoSave;
    }
    private static void Initialize()
    {
        var datasByType = DataTableManager.SoldierTable.GetTypeDictionary();

        foreach (var type in datasByType.Keys) 
        {
            InventorySaveData inventoryData = new InventorySaveData();
            inventoryData.inventoryType = type;

            foreach(var soldierData in datasByType[type])
            {
                InventoryElementSaveData elementData = new InventoryElementSaveData()
                {
                    soldierId = soldierData.ID,
                    isLocked = true,
                    grade = soldierData.Rating,
                    count = 0,
                    level = 0
                };
                inventoryData.elements.Add(elementData);
            }
            inventoryData.elements[0].isLocked = false;
            inventoryData.elements[0].count = 1;
            inventoryData.equipElementID = inventoryData.elements[0].soldierId;
            inventories[type] = inventoryData;
        }
        onChangedInventory?.Invoke();
    }
    public static InventorySaveData GetInventoryData(UnitTypes type)
    {
        if (inventories.ContainsKey(type))
        {
            return inventories[type];
        }
        return null;
    }
    public static void Add(List<SoldierTable.Data> datas)
    {
        foreach(var data in datas) 
        {
            Add(data);
        }
    }
    public static void Add(SoldierTable.Data data)
    {
        if (!inventories.ContainsKey(data.Kind))
        {
            return;
        }

        InventorySaveData inventoryData = inventories[data.Kind];

        InventoryElementSaveData element = inventoryData.elements.Find(e => e.soldierId == data.ID);

        if (element != null)
        {
            if (element.isLocked)
            {
                element.isLocked = false;
                element.count = 1;
            }
            else
            {
                element.count++;
            }
        }
        onChangedInventory?.Invoke();
    }
    public static void Merge(int soldierId, int count = 1)
    {
        SoldierTable.Data data = DataTableManager.SoldierTable.GetData(soldierId);
        InventorySaveData inventoryData = GetInventoryData(data.Kind);
        if(inventoryData == null) 
        {
            return;
        }

        int index = inventoryData.elements.FindIndex((e) => e.soldierId == data.ID);
        if(index < 0 || index >= inventoryData.elements.Count)
        {
            return;
        }

        InventoryElementSaveData currentElement = inventoryData.elements[index];
        if (currentElement.count < requireMergeCount * count)
        {
            return;
        }

        currentElement.count -= requireMergeCount * count;

        InventoryElementSaveData nextElement = inventoryData.elements[index + 1];
        if (nextElement.isLocked)
        {
            nextElement.isLocked = false;
            nextElement.count = count;
        }
        else
        {
            nextElement.count += count;
        }
        onChangedInventory?.Invoke();
    }

    private static void DoSave(TotalSaveData totalSaveData)
    {
        totalSaveData.inventorySaveData = inventories;
    }
    public static void DoLoad()
    {
        inventories = new Dictionary<UnitTypes, InventorySaveData>(SaveLoadManager.LoadedData.inventorySaveData);
    }
}
