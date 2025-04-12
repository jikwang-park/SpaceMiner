using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class InventoryManager
{
    private static Dictionary<UnitTypes, SoldierInventoryData> Inventories
    {
        get
        {
            return SaveLoadManager.Data.soldierInventorySaveData;
        }
    }
    public static readonly int requireMergeCount = 5;
    public static event Action onChangedInventory;
    public static void Initialize()
    {
        var datasByType = DataTableManager.SoldierTable.GetTypeDictionary();

        foreach (var type in datasByType.Keys) 
        {
            SoldierInventoryData inventoryData = new SoldierInventoryData();
            inventoryData.inventoryType = type;

            foreach(var soldierData in datasByType[type])
            {
                SoldierInventoryElementData elementData = new SoldierInventoryElementData()
                {
                    soldierId = soldierData.ID,
                    isLocked = true,
                    grade = soldierData.Grade,
                    count = 0,
                    level = 0
                };
                inventoryData.elements.Add(elementData);
            }
            inventoryData.elements[0].isLocked = false;
            inventoryData.elements[0].count = 1;
            inventoryData.equipElementID = inventoryData.elements[0].soldierId;
            if(!Inventories.ContainsKey(type))
            {
                Inventories[type] = inventoryData;
            }
        }
        onChangedInventory?.Invoke();
    }
    public static SoldierInventoryData GetInventoryData(UnitTypes type)
    {
        return Inventories.ContainsKey(type) ? Inventories[type] : null;
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
        if (!Inventories.ContainsKey(data.UnitType))
        {
            return;
        }

        SoldierInventoryData inventoryData = Inventories[data.UnitType];

        SoldierInventoryElementData element = inventoryData.elements.Find(e => e.soldierId == data.ID);

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
        SoldierInventoryData inventoryData = GetInventoryData(data.UnitType);
        if(inventoryData == null) 
        {
            return;
        }

        int index = inventoryData.elements.FindIndex((e) => e.soldierId == data.ID);
        if(index < 0 || index >= inventoryData.elements.Count)
        {
            return;
        }

        SoldierInventoryElementData currentElement = inventoryData.elements[index];
        if (currentElement.count < requireMergeCount * count)
        {
            return;
        }

        currentElement.count -= requireMergeCount * count;

        SoldierInventoryElementData nextElement = inventoryData.elements[index + 1];
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
    public static bool IsExist(UnitTypes type, Grade grade)
    {
         return Inventories[type].elements.Where((e) => e.grade == grade && !e.isLocked).Count() > 0;
    }
}
