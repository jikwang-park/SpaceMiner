using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitStatsUpgradeManager 
{
    private static Dictionary<UnitUpgradeTable.UpgradeType,int> unitStatsDictionary
    {
        get
        {
            return SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;
        }
    }
    
    public static void SetLevel(UnitUpgradeTable.UpgradeType type, int level)
    {
        unitStatsDictionary[type] = level;
    }

    public static Dictionary<UnitUpgradeTable.UpgradeType,int> GetCurrentData()
    {
        return unitStatsDictionary;
    }
}
