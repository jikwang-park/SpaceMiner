using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string StringID { get; set; }
        public int Planet { get; set; }
        public int Stage { get; set; }
        public string CorpsID { get; set; }
        public int FirstClearReward { get; set; }
        public int IdleReward { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            StringID = argument[1];
            Planet = int.Parse(argument[2]);
            Stage = int.Parse(argument[3]);
            CorpsID = argument[4];
            FirstClearReward = int.Parse(argument[5]);
            IdleReward = int.Parse(argument[6]);
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    private Dictionary<int, Dictionary<int, Data>> planetDict = new Dictionary<int, Dictionary<int, Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        var list = LoadCsv<Data>(text);
        dict.Clear();
        TableData.Clear();
        planetDict.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.ID))
            {
                dict.Add(item.ID, item);
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

    public bool IsExistPlanet(string key)
    {
        return dict.ContainsKey(key);
    }

    public bool IsExistPlanet(int planet)
    {
        return planetDict.ContainsKey(planet);
    }

    public bool IsExistStage(int planet, int stage)
    {
        return IsExistPlanet(planet) && planetDict[planet].ContainsKey(stage);
    }

    public Data GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
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
        var dictionary = new Dictionary<string, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        var stageDict = new Dictionary<int, Dictionary<int, Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID, datum);
            if (!stageDict.ContainsKey(datum.Planet))
            {
                stageDict.Add(datum.Planet, new Dictionary<int, Data>());
            }
            stageDict[datum.Planet].Add(datum.Stage, datum);
        }
        dict = dictionary;
        TableData = tableData;
        this.planetDict = stageDict;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
