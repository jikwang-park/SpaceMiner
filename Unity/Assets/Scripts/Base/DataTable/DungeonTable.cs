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
        public int DungeonKeyID { get; set; }
        public int KeyCount { get; set; }
        public int ItemID { get; set; }
        public int FirstClearReward { get; set; }
        public int ClearReward { get; set; }
        public int ConditionPlanet { get; set; }
        public int ConditionPower { get; set; }
        public int KeyPoint { get; set; }
        public float LimitTime { get; set; }
        public int WaveCorpsID { get; set; }
        public string PrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = int.Parse(argument[1]);
            NameStringID = int.Parse(argument[2]);
            Stage = int.Parse(argument[3]);
            DungeonKeyID = int.Parse(argument[4]);
            KeyCount = int.Parse(argument[5]);
            ItemID = int.Parse(argument[6]);
            FirstClearReward = int.Parse(argument[7]);
            ClearReward = int.Parse(argument[8]);
            ConditionPlanet = int.Parse(argument[9]);
            ConditionPower = int.Parse(argument[10]);
            KeyPoint = int.Parse(argument[11]);
            LimitTime = float.Parse(argument[12]);
            WaveCorpsID = int.Parse(argument[13]);
            PrefabID = argument[14];
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
        foreach(var data in typeDict[type])
        {
            if(data.Stage == stage)
            {
                return data;
            }
        }
        return null;
    }

    public List<int> DungeonTypes => types;

    public int CountOfStage(int type)
    {
        if(typeDict.ContainsKey(type))
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
