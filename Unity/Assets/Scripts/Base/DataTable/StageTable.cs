using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int StringID { get; set; }
        public int Planet { get; set; }
        public int Stage { get; set; }
        public int CorpsID { get; set; }
        public int FirstClearRewardID { get; set; }
        public string FirstClearRewardCount { get; set; }
        public int IdleRewardID { get; set; }
        public string IdleRewardCount { get; set; }
        public string PrefabId { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            StringID = int.Parse(argument[1]);
            Planet = int.Parse(argument[2]);
            Stage = int.Parse(argument[3]);
            CorpsID = int.Parse(argument[4]);
            FirstClearRewardID = int.Parse(argument[5]);
            FirstClearRewardCount = argument[6];
            IdleRewardID = int.Parse(argument[7]);
            IdleRewardCount = argument[8];
            PrefabId = argument[9];
        }
    }

    private Dictionary<int, Dictionary<int, Data>> planetDict = new Dictionary<int, Dictionary<int, Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        planetDict.Clear();

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
                if (!planetDict.ContainsKey(item.Planet))
                {
                    planetDict.Add(item.Planet, new Dictionary<int, Data>());
                }
                planetDict[item.Planet].Add(item.Stage, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public bool Contains(int key)
    {
        return TableData.ContainsKey(key);
    }

    public bool IsExistPlanet(int planet)
    {
        return planetDict.ContainsKey(planet);
    }

    public bool IsExistStage(int planet, int stage)
    {
        return IsExistPlanet(planet) && planetDict[planet].ContainsKey(stage);
    }

    public Data GetData(int key)
    {
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return (Data)TableData[key];
    }

    public List<Data> GetPlanetData(int planet)
    {
        return planetDict[planet].Values.ToList();
    }

    public List<int> GetPlanetKeys()
    {
        return planetDict.Keys.ToList();
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var stageDict = new Dictionary<int, Dictionary<int, Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            if (!stageDict.ContainsKey(datum.Planet))
            {
                stageDict.Add(datum.Planet, new Dictionary<int, Data>());
            }
            stageDict[datum.Planet].Add(datum.Stage, datum);
        }
        TableData = tableData;
        planetDict = stageDict;
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
