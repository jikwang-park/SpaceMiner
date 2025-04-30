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
        public int NameStringID { get; set; }
        public int Planet { get; set; }
        public int Stage { get; set; }
        public int WaveID { get; set; }
        public int FirstClearRewardID { get; set; }
        public string FirstClearRewardCount { get; set; }
        public int IdleRewardItemID { get; set; }
        public string IdleRewardItemCount { get; set; }
        public int PrefabID { get; set; }
        public float AtkWeight { get; set; }
        public float HpWeight { get; set; }
        public float GoldWeight { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            Planet = int.Parse(argument[2]);
            Stage = int.Parse(argument[3]);
            WaveID = int.Parse(argument[4]);
            FirstClearRewardID = int.Parse(argument[5]);
            FirstClearRewardCount = argument[6];
            IdleRewardItemID = int.Parse(argument[7]);
            IdleRewardItemCount = argument[8];
            PrefabID = int.Parse(argument[9]);
            AtkWeight = float.Parse(argument[10]);
            HpWeight = float.Parse(argument[11]);
            GoldWeight = float.Parse(argument[12]);
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

    public Data GetStageData(int planet, int stage)
    {
        return planetDict[planet][stage];
    }

    public List<int> GetPlanetKeys()
    {
        return planetDict.Keys.ToList();
    }

    public Data GetLastStage()
    {
        var maxplanet = planetDict.Keys.Max();
        var maxstage = planetDict[maxplanet].Max(p => p.Value.Stage);
        return GetStageData(maxplanet, maxstage);
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
