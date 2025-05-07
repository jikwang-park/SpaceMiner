using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectItemInventoryManager
{
    public static Dictionary<EffectItemTable.ItemType, int> EffectItemInventory
    {
        get
        {
            return SaveLoadManager.Data.possessionEffectItemDatas;
        }
    }
    static EffectItemInventoryManager()
    {
        ItemManager.OnGainEffectItem += DoGainEffectItem;
    }
    public static void DoGainEffectItem(int itemId)
    {
        EffectItemTable.ItemType effectType;
        foreach (var type in Enum.GetValues(typeof(EffectItemTable.ItemType)))
        {
            int needItemId = DataTableManager.EffectItemTable.GetDatas((EffectItemTable.ItemType)type)[0].NeedItemID;
            if (itemId == needItemId)
            {
                effectType = (EffectItemTable.ItemType)type;
                break;
            }
        }

    }
    public static bool LevelUp(EffectItemTable.ItemType type)
    {
        int level = GetLevel(type);
        var dataList = DataTableManager.EffectItemTable.GetDatas(type);
        if(level + 1 >= dataList.Count)
        {
            return false;
        }
        
        var currentLevelData = dataList[level];
        if(!ItemManager.CanConsume(currentLevelData.NeedItemID, currentLevelData.NeedItemCount))
        {
            return false;
        }

        ItemManager.ConsumeItem(currentLevelData.NeedItemID, currentLevelData.NeedItemCount);
        EffectItemInventory[type]++;
        return true;
    }
    public static int GetLevel(EffectItemTable.ItemType type)
    {
        return EffectItemInventory[type];
    }
}
