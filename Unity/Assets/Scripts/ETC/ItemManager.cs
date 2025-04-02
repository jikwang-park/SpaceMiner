using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ItemManager
{
    private static Dictionary<int, BigNumber> items
    {
        get
        {
            return SaveLoadManager.Data.itemSaveData;
        }
    }
    public static void AddItem(int itemId, BigNumber amount)
    {
        BigNumber maxStack = DataTableManager.ItemTable.GetData(itemId).MaxStack;
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
    }
    public static void ConsumeItem(int itemId, BigNumber amount)
    {
        if(CanConsume(itemId, amount))
        {
            items[itemId] -= amount;
        }
    }
    public static bool CanConsume(int itemId, BigNumber amount)
    {
        return (items.ContainsKey(itemId) && items[itemId] >= amount);
    }
    public static void ConsumeCurrency(Currency currency, BigNumber amount)
    {
        ConsumeItem((int)currency, amount);
    }
    public static BigNumber GetItemAmount(int itemId)
    {
        return items.ContainsKey(itemId)? items[itemId] : 0;
    }
}
