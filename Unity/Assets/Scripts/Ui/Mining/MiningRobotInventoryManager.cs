using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiningRobotInventoryManager
{
    public static event Action<int, MiningRobotInventorySlotData> onChangedInventory;
    public static MiningRobotInventoryData Inventory
    {
        get
        {
            return SaveLoadManager.Data.miningRobotInventorySaveData;
        }
    }

    public static void AddRobot(int robotId)
    {
        for(int i = 0; i < Inventory.slots.Count; i++)
        {
            if (Inventory.slots[i].isEmpty)
            {
                Inventory.slots[i].isEmpty = false;
                Inventory.slots[i].miningRobotId = robotId;
                onChangedInventory?.Invoke(i, Inventory.slots[i]);
                return;
            }
        }
    }
}
