using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ItemManager
{
    public static event Action<int, BigNumber> OnItemAmountChanged;
    public static event Action<int> OnGainEffectItem;
    private static Dictionary<int, BigNumber> items
    {
        get
        {
            return SaveLoadManager.Data.itemSaveData;
        }
    }
    private static float GoldIncreaseRatio
    {
        get
        {
            int goldUpgradeLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.Gold];
            float goldUpgradeValue = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.Gold)[goldUpgradeLevel].Value;

            int goldEffectItemLevel = EffectItemInventoryManager.GetLevel(EffectItemTable.ItemType.GoldGain);
            float goldEffectItemValue = DataTableManager.EffectItemTable.GetDatas(EffectItemTable.ItemType.GoldGain)[goldEffectItemLevel].Value;

            return 1 + goldUpgradeValue + goldEffectItemValue;
        }
    }
    private static float MiningIncreaseRatio
    {
        get
        {
            int miningUpgradeLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.Mining];
            float miningUpgradeValue = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.Mining)[miningUpgradeLevel].Value;

            int resourceEffectItemLevel = EffectItemInventoryManager.GetLevel(EffectItemTable.ItemType.ResourceGain);
            float resourceEffectItemValue = DataTableManager.EffectItemTable.GetDatas(EffectItemTable.ItemType.ResourceGain)[resourceEffectItemLevel].Value;

            return 1 + miningUpgradeValue + resourceEffectItemValue;
        }
    }
    public static void Clear()
    {
        OnItemAmountChanged = null;

    }
    public static bool AddItem(int itemId, BigNumber amount)
    {
        if(DataTableManager.ItemTable.GetData(itemId) == null)
        {
            return false;
        }

        if(Enum.IsDefined(typeof(Currency), itemId))
        {
            var currency = (Currency)itemId;
            switch (currency)
            {
                case Currency.Gold:
                    amount *= GoldIncreaseRatio;
                    break;
                case Currency.Annotaion:
                case Currency.Cobalt:
                case Currency.Tungsten:
                case Currency.Titanium:
                case Currency.Spinel:
                    amount *= MiningIncreaseRatio;
                    break;
            }
        }

        BigNumber maxStack = DataTableManager.ItemTable.GetData(itemId).MaxStack.ToString();
        if(items.ContainsKey(itemId))
        {
            items[itemId] += amount;
        }
        else
        {
            items[itemId] = amount;
        }

        if (items[itemId] > maxStack)
        {
            items[itemId] = maxStack;
        }

        GuideQuestManager.QuestProgressChange(GuideQuestTable.MissionType.Item);
        OnItemAmountChanged?.Invoke(itemId, items[itemId]);
        if (DataTableManager.ItemTable.GetData(itemId).ItemType == 4)
        {
            OnGainEffectItem?.Invoke(itemId);
        }

        return true;
    }
    public static bool ConsumeItem(int itemId, BigNumber amount)
    {
        if(CanConsume(itemId, amount))
        {
            items[itemId] -= amount;
            OnItemAmountChanged?.Invoke(itemId, items[itemId]);
            return true;
        }
        return false;
    }
    public static bool CanConsume(int itemId, BigNumber amount)
    {
        return (items.ContainsKey(itemId) && items[itemId] >= amount);
    }
    public static void ConsumeCurrency(Currency currency, BigNumber amount)
    {
        ConsumeItem((int)currency, amount);
    }
    public static void AddCurrency(Currency currency, BigNumber amount)
    {
        AddItem((int)currency, amount);
    }
    public static BigNumber GetItemAmount(int itemId)
    {
        return items.ContainsKey(itemId)? items[itemId] : 0;
    }
}
