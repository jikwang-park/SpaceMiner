using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GachaSoldierTable : DataTable
{
    public class Data : DataTableData
    {
        public string gachaID { get; set; }
        public string grade { get; set; }
        public string soldierID { get; set; }
        public float probability { get; set; }

        public override void Set(string[] argument)
        {
            gachaID = argument[0];
            grade = argument[1];
            soldierID = argument[2];
            probability = float.Parse(argument[3]);
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    private Dictionary<string, List<Data>> gradeDict = new Dictionary<string, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();
        gradeDict.Clear();

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
                if (!gradeDict.ContainsKey(item.grade))
                {
                    gradeDict.Add(item.grade, new List<Data>());
                }
                gradeDict[item.grade].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.gachaID}");
            }
        }
    }

    public List<Data> GetGradeDatas(string grade)
    {
        if (gradeDict.ContainsKey(grade))
        {
            return gradeDict[grade];
        }
        return null;
    }

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<string, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        var gradeDict = new Dictionary<string, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.gachaID, datum);
            tableData.Add(datum.gachaID, datum);
            if (!gradeDict.ContainsKey(datum.grade))
            {
                gradeDict.Add(datum.grade, new List<Data>());
            }
            gradeDict[datum.grade].Add(datum);
        }
        dict = dictionary;
        TableData = tableData;
        this.gradeDict = gradeDict;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
