using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectItemInventoryManager
{
    public static event Action<int> OnEffectItemLevelUp;
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
    public static void Clear()
    {
        OnEffectItemLevelUp = null;
    }
    public static void DoGainEffectItem(int itemId)
    {
        var effectType = DataTableManager.EffectItemTable.GetTypeByID(itemId);
        int count = 0;
        while (LevelUp(effectType))
        {
            ++count;
        }

        if(count > 0)
        {
            OnEffectItemLevelUp?.Invoke(itemId);
            UnitCombatPowerCalculator.CalculateTotalCombatPower();
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
