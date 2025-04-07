using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiningRobotInventoryManager
{
    public static MiningRobotInventoryData Inventory
    {
        get
        {
            return SaveLoadManager.Data.miningRobotInventorySaveData;
        }
    }

    public static void AddRobot(int robotId)
    {
        foreach(var slot in Inventory.slots)
        {
            if(slot.isEmpty)
            {
                slot.isEmpty = false;
                slot.miningRobotId = robotId;
                return;
            }
        }
    }
}
