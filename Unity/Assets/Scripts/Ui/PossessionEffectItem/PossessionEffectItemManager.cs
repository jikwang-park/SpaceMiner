using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PossessionEffectItemManager
{
    public static Dictionary<PossessionEffectType, int> PossessionEffectItems
    {
        get
        {
            return SaveLoadManager.Data.possessionEffectItemDatas;
        }
    }
    static PossessionEffectItemManager()
    {
        ItemManager.OnGainEffectItem += DoGainEffectItem;
    }
    public static void DoGainEffectItem(int itemId)
    {

    }
    public static bool LevelUp(PossessionEffectType type)
    {

        PossessionEffectItems[type]++;
        return true;
    }
}
