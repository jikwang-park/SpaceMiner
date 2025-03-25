using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        LoadTables();
    }

    private static void LoadTables()
    {
#if UNITY_EDITOR
        foreach (var id in DataTableIds.stringTables)
        {
            var table = new StringTable();
            table.Load(id);
            tables.Add(id, table);
        }
#else
        {
            var table = new StringTable();
            var stringTableId = DataTableIds.stringTables[(int)Variables.currentLanguage];
            table.Load(stringTableId);
            tables.Add(stringTableId, table);
        }
#endif

        {
            var table = new CorpsTable();
            table.Load(DataTableIds.corpsTable);
            tables.Add(DataTableIds.corpsTable, table);
        }
        {
            var table = new StageTable();
            table.Load(DataTableIds.stageTable);
            tables.Add(DataTableIds.stageTable, table);
        }
        {
            var table = new WaveTable();
            table.Load(DataTableIds.waveTable);
            tables.Add(DataTableIds.waveTable, table);
        }
        {
            var table = new SoldierTable();
            table.Load(DataTableIds.soldierTable);
            tables.Add(DataTableIds.soldierTable, table);
        }
    }


    public static T GetTable<T>(string tableId) where T : DataTable
    {
        bool isContainsKey = tables.ContainsKey(tableId);
        if (!isContainsKey)
        {
            Debug.Assert(isContainsKey, "Table Not Exists");
            return null;
        }
        return tables[tableId] as T;
    }

    public static StringTable StringTable
    {
        get
        {
            return GetTable<StringTable>(DataTableIds.stringTables[(int)Variables.currentLanguage]);
        }
    }

    public static CorpsTable CorpsTable
    {
        get
        {
            return GetTable<CorpsTable>(DataTableIds.corpsTable);
        }
    }

    public static StageTable StageTable
    {
        get
        {
            return GetTable<StageTable>(DataTableIds.stageTable);
        }
    }

    public static WaveTable WaveTable
    {
        get
        {
            return GetTable<WaveTable>(DataTableIds.waveTable);
        }
    }
    public static SoldierTable SoldierTable
    {
        get
        {
            return GetTable<SoldierTable>(DataTableIds.soldierTable);
        }
    }
}
