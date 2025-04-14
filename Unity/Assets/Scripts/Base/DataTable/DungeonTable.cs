using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int NameStringID { get; set; }
        public int Stage { get; set; }
        public int NeedKeyItemID { get; set; }
        public int NeedKeyItemCount { get; set; }
        public int RewardItemID { get; set; }
        public int FirstClearRewardItemCount { get; set; }
        public int ClearRewardItemCount { get; set; }
        public int NeedClearPlanet { get; set; }
        public int NeedPower { get; set; }
        public int KeyPoint { get; set; }
        public float LimitTime { get; set; }
        public int WaveID { get; set; }
        public int PrefabID { get; set; }
        public int SpriteID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = int.Parse(argument[1]);
            NameStringID = int.Parse(argument[2]);
            Stage = int.Parse(argument[3]);
            NeedKeyItemID = int.Parse(argument[4]);
            NeedKeyItemCount = int.Parse(argument[5]);
            RewardItemID = int.Parse(argument[6]);
            FirstClearRewardItemCount = int.Parse(argument[7]);
            ClearRewardItemCount = int.Parse(argument[8]);
            NeedClearPlanet = int.Parse(argument[9]);
            NeedPower = int.Parse(argument[10]);
            KeyPoint = int.Parse(argument[11]);
            LimitTime = float.Parse(argument[12]);
            WaveID = int.Parse(argument[13]);
            PrefabID = int.Parse(argument[14]);
            SpriteID = int.Parse(argument[15]);
        }
    }

    public override Type DataType => typeof(Data);

    private Dictionary<int, List<Data>> typeDict = new Dictionary<int, List<Data>>();
    private List<int> types = new List<int>();

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        typeDict.Clear();
        types.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

        foreach (var item in list)
        {
            if (!TableData.ContainsKey(item.ID))
            {
                TableData.Add(item.ID, item);

                if (!typeDict.ContainsKey(item.Type))
                {
                    typeDict.Add(item.Type, new List<Data>());
                    types.Add(item.Type);
                }
                typeDict[item.Type].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newTypes = new List<int>();
        var newTypeDict = new Dictionary<int, List<Data>>();

        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!newTypeDict.ContainsKey(datum.Type))
            {
                newTypeDict.Add(datum.Type, new List<Data>());
                newTypes.Add(datum.Type);
            }
            newTypeDict[datum.Type].Add(datum);
        }
        TableData = tableData;
        types = newTypes;
        typeDict = newTypeDict;
    }

    public Data GetData(int key)
    {
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return (Data)TableData[key];
    }

    public Data GetData(int type, int stage)
    {
        foreach (var data in typeDict[type])
        {
            if (data.Stage == stage)
            {
                return data;
            }
        }
        return null;
    }

    public List<int> DungeonTypes => types;

    public int CountOfStage(int type)
    {
        if (typeDict.ContainsKey(type))
        {
            return typeDict[type].Count;
        }
        return 0;
    }

    public List<Data> GetDungeonList(int type)
    {
        if (typeDict.ContainsKey(type))
        {
            return typeDict[type];
        }
        return null;
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
