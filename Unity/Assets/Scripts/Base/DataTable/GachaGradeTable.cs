using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaGradeTable : DataTable
{
    public class Data : DataTableData
    {
        public string gachaID { get; set; }
        public int gachaLevel { get; set; }
        public string grade { get; set; }
        public float probability { get; set; }

        public override void Set(string[] argument)
        {
            gachaID = argument[0];
            gachaLevel = int.Parse(argument[1]);
            grade = argument[2];
            probability = float.Parse(argument[3]);
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    private Dictionary<int, List<Data>> levelDict = new Dictionary<int, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();
        levelDict.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.gachaID))
            {
                dict.Add(item.gachaID, item);
                TableData.Add(item.gachaID, item);
                if (!levelDict.ContainsKey(item.gachaLevel))
                {
                    levelDict.Add(item.gachaLevel, new List<Data>());
                }
                levelDict[item.gachaLevel].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.gachaID}");
            }
        }
    }

    public List<Data> GetLevelData(int level)
    {
        if (levelDict.ContainsKey(level))
        {
            return levelDict[level];
        }
        return null;
    }

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<string, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        var levelDict = new Dictionary<int, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.gachaID, datum);
            tableData.Add(datum.gachaID, datum);

            if (!levelDict.ContainsKey(datum.gachaLevel))
            {
                levelDict.Add(datum.gachaLevel, new List<Data>());
            }
            levelDict[datum.gachaLevel].Add(datum);
        }
        dict = dictionary;
        TableData = tableData;
        this.levelDict = levelDict;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
