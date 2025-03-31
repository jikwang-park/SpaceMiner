using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SoldierTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int StringID { get; set; }
        public UnitTypes Kind { get; set; }
        public Grade Rating { get; set; }
        public float Basic_AP { get; set; }
        public float Basic_HP { get; set; }
        public float Basic_DP { get; set; }
        public float Special_DR { get; set; }
        public float Special_CD { get; set; }
        public float Special_H { get; set; }
        public int IncreaseID { get; set; }
        public int BuildingID { get; set; }
        public int SkillID { get; set; }
        public float Distance { get; set; }
        public float MoveSpeed { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            StringID = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int kind))
            {
                Kind = (UnitTypes)kind;
            }
            else
            {
                Kind = Enum.Parse<UnitTypes>(argument[2]);
            }
            if (int.TryParse(argument[3], out int rating))
            {
                Rating = (Grade)rating;
            }
            else
            {
                Rating = Enum.Parse<Grade>(argument[3]);
            }
            Basic_AP = float.Parse(argument[4]);
            Basic_HP = float.Parse(argument[5]);
            Basic_DP = float.Parse(argument[6]);
            Special_DR = float.Parse(argument[7]);
            Special_CD = float.Parse(argument[8]);
            Special_H = float.Parse(argument[9]);
            IncreaseID = int.Parse(argument[10]);
            BuildingID = int.Parse(argument[11]);
            SkillID = int.Parse(argument[12]);
            Distance = float.Parse(argument[13]);
            MoveSpeed = float.Parse(argument[14]);
        }
    }

    private Dictionary<UnitTypes, List<Data>> typeDict = new Dictionary<UnitTypes, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        typeDict.Clear();
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
                TableData.Add(item.ID, item);

                if (!typeDict.ContainsKey(item.Kind))
                {
                    typeDict[item.Kind] = new List<Data>();
                }
                typeDict[item.Kind].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }

        }
    }
    
    public Dictionary<UnitTypes, List<Data>> GetTypeDictionary()
    {
        return typeDict;
    }

    public Data GetData(int key)
    {
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return (Data)TableData[key];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var typeDict = new Dictionary<UnitTypes, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!typeDict.ContainsKey(datum.Kind))
            {
                typeDict[datum.Kind] = new List<Data>();
            }
            typeDict[datum.Kind].Add(datum);
        }
        TableData = tableData;
        this.typeDict = typeDict;
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

