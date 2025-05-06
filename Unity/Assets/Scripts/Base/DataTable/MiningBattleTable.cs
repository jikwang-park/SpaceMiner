using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MiningBattleTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public int DetailStringID { get; set; }
        public int SpawnTableID { get; set; }
        public float LimitTime { get; set; }
        public int Reward1ItemID { get; set; }
        public string Reward1ItemCount { get; set; }
        public int Reward2ItemID { get; set; }
        public string Reward2ItemCount { get; set; }
        public float Reward2ItemProbability { get; set; }
        public int PlanetTableID { get; set; }
        public int Stage { get; set; }
        public int HitCount { get; set; }
        public int SpriteID { get; set; }
        public int PrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            DetailStringID = int.Parse(argument[2]);
            SpawnTableID = int.Parse(argument[3]);
            LimitTime = float.Parse(argument[4]);
            Reward1ItemID = int.Parse(argument[5]);
            Reward1ItemCount = argument[6];
            Reward2ItemID = int.Parse(argument[7]);
            Reward2ItemCount = argument[8];
            Reward2ItemProbability = float.Parse(argument[9]);
            PlanetTableID = int.Parse(argument[10]);
            Stage = int.Parse(argument[11]);
            HitCount = int.Parse(argument[12]);
            SpriteID = int.Parse(argument[13]);
            PrefabID = int.Parse(argument[14]);
        }
    }

    public override Type DataType => typeof(Data);

    private Dictionary<int, List<Data>> planetStages = new Dictionary<int, List<Data>>();

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
                if (!planetStages.ContainsKey(item.PlanetTableID))
                {
                    planetStages.Add(item.PlanetTableID, new List<Data>());
                }
                planetStages[item.PlanetTableID].Add(item);

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
            throw new ArgumentException("ID Not Found");
        }
        return TableData[key] as Data;
    }

    public List<Data> GetDatas(int tableId)
    {
        if (!planetStages.ContainsKey(tableId))
        {
            throw new ArgumentException("PlanetTableID Not Found");
        }

        return planetStages[tableId];
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
