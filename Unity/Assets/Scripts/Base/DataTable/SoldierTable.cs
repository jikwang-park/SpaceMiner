using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
public enum Grade
{
    Normal = 1,
    Rare,
    Epic,
    Legend,
}
public class SoldierTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string StringID { get; set; }
        public UnitTypes Kind { get; set; }
        public Grade Rating { get; set; }
        public float Basic_AP { get; set; }
        public float Basic_HP { get; set; }
        public float Basic_DP { get; set; }
        public float Special_DR { get; set; }
        public float Special_CD { get; set; }
        public float Special_H { get; set; }
        public string IncreaseID { get; set; }
        public string BuildingID { get; set; }
        public string SkillID { get; set; }
        public float Distance { get; set; }
        public float MoveSpeed { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            StringID = argument[1];
            Kind = (UnitTypes)int.Parse(argument[2]);
            Rating = (Grade)int.Parse(argument[3]);
            Basic_AP = float.Parse(argument[4]);
            Basic_HP = float.Parse(argument[5]);
            Basic_DP = float.Parse(argument[6]);
            Special_DR = float.Parse(argument[7]);
            Special_CD = float.Parse(argument[8]);
            Special_H = float.Parse(argument[9]);
            IncreaseID = argument[10];
            BuildingID = argument[11];
            SkillID = argument[12];
            Distance = float.Parse(argument[13]);
            MoveSpeed = float.Parse(argument[14]);
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    private Dictionary<UnitTypes, List<Data>> typeDict = new Dictionary<UnitTypes, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        var list = LoadCsv<Data>(text);
        dict.Clear();
        typeDict.Clear();
        TableData.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.ID))
            {
                dict.Add(item.ID, item);
                TableData.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }

            if (!typeDict.ContainsKey(item.Kind))
            {
                typeDict[item.Kind] = new List<Data>();
            }
            typeDict[item.Kind].Add(item);
        }
    }
    public Dictionary<UnitTypes, List<Data>> GetTypeDictionary()
    {
        return typeDict;
    }
    public Data GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<string, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        var typeDict = new Dictionary<UnitTypes, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID, datum);

            if (!typeDict.ContainsKey(datum.Kind))
            {
                typeDict[datum.Kind] = new List<Data>();
            }
            typeDict[datum.Kind].Add(datum);
        }
        dict = dictionary;
        TableData = tableData;
        this.typeDict = typeDict;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}

