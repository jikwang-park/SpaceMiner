using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GachaSoldierTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public Grade Grade { get; set; }
        public int SoldierID { get; set; }
        public float Probability { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Grade = (Grade)type;
            }
            else
            {
                Grade = Enum.Parse<Grade>(argument[1]);
            }
            SoldierID = int.Parse(argument[2]);
            Probability = float.Parse(argument[3]);
        }
    }

    private Dictionary<Grade, List<Data>> gradeDict = new Dictionary<Grade, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        gradeDict.Clear();

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
                if (!gradeDict.ContainsKey(item.Grade))
                {
                    gradeDict.Add(item.Grade, new List<Data>());
                }
                gradeDict[item.Grade].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public List<Data> GetGradeDatas(Grade grade)
    {
        if (gradeDict.ContainsKey(grade))
        {
            return gradeDict[grade];
        }
        return null;
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var gradeDict = new Dictionary<Grade, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            if (!gradeDict.ContainsKey(datum.Grade))
            {
                gradeDict.Add(datum.Grade, new List<Data>());
            }
            gradeDict[datum.Grade].Add(datum);
        }
        TableData = tableData;
        this.gradeDict = gradeDict;
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
