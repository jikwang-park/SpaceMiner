using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaGradeTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int GachaID { get; set; }
        public Grade Grade { get; set; }
        public float Probability { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            GachaID = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int type))
            {
                Grade = (Grade)type;
            }
            else
            {
                Grade = Enum.Parse<Grade>(argument[1]);
            }
            Probability = float.Parse(argument[3]);
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
            if (!TableData.ContainsKey(item.GachaID))
            {
                TableData.Add(item.ID, item);
                if (!levelDict.ContainsKey(item.GachaID))
                {
                    levelDict.Add(item.GachaID, new List<Data>());
                }
                levelDict[item.GachaID].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
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
            tableData.Add(datum.ID, datum);

            if (!levelDict.ContainsKey(datum.GachaID))
            {
                levelDict.Add(datum.GachaID, new List<Data>());
            }
            levelDict[datum.GachaID].Add(datum);
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
