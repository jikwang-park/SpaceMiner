using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MiningBattleSpawnTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string SpawnMonsterID { get; set; }
        public float SpawnInterval { get; set; }
        public float HPIncreasement { get; set; }
        public float SpawnIntervalReduction { get; set; }
        public string SpawnerActivationTime { get; set; }

        public int[] SpawnMonsterIDs;
        public float[] SpawnerActivationTimes;


        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);

            SpawnMonsterID = argument[1];
            SpawnMonsterIDs = DataTableUtilities.SplitColumnToInt(argument[1]);
            SpawnInterval = float.Parse(argument[2]);
            HPIncreasement = float.Parse(argument[3]);
            SpawnIntervalReduction = float.Parse(argument[4]);
            SpawnerActivationTime = argument[5];
            SpawnerActivationTimes = DataTableUtilities.SplitColumnToFloat(argument[5]);
        }
    }

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

        foreach (var item in list)
        {
            if (!TableData.ContainsKey(item.ID))
            {
                item.SpawnMonsterIDs = DataTableUtilities.SplitColumnToInt(item.SpawnMonsterID);
                item.SpawnerActivationTimes = DataTableUtilities.SplitColumnToFloat(item.SpawnerActivationTime);
                TableData.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int key)
    {
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return TableData[key] as Data;
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
        }
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        List<Data> list = new List<Data>();

        foreach (var item in TableData)
        {
            list.Add((Data)item.Value);
        }

        return CreateCsv(list);
    }
}
