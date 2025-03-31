using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaGradeTable : DataTable
{
    public class Data : ITableData
    {
        public int gachaID { get; set; }
        public int gachaLevel { get; set; }
        public string grade { get; set; }
        public float probability { get; set; }

        public void Set(string[] argument)
        {
            gachaID = int.Parse(argument[0]);
            gachaLevel = int.Parse(argument[1]);
            grade = argument[2];
            probability = float.Parse(argument[3]);
        }
    }

    private Dictionary<int, List<Data>> levelDict = new Dictionary<int, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        levelDict.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

        foreach (var item in list)
        {
            if (!TableData.ContainsKey(item.gachaID))
            {
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
        var tableData = new Dictionary<int, ITableData>();
        var levelDict = new Dictionary<int, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.gachaID, datum);

            if (!levelDict.ContainsKey(datum.gachaLevel))
            {
                levelDict.Add(datum.gachaLevel, new List<Data>());
            }
            levelDict[datum.gachaLevel].Add(datum);
        }
        TableData = tableData;
        this.levelDict = levelDict;
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
