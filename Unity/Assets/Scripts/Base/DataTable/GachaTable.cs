using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class GachaTable : DataTable
{
    public class Data : ITableData
    {
        public int gachaID { get; set; }
        public string nameStringID { get; set; }
        public string cost_ItemID { get; set; }
        public int cost { get; set; }
        public string cost_Item2ID { get; set; }
        public int cost2 { get; set; }
        public int repeat { get; set; }
        public int repeat2 { get; set; }
        public int growRate { get; set; }
        public string explainStringID { get; set; }

        public void Set(string[] argument)
        {
            gachaID = int.Parse(argument[0]);
            nameStringID = argument[1];
            cost_ItemID = argument[2];
            cost = int.Parse(argument[3]);
            cost_Item2ID = argument[4];
            cost2 = int.Parse(argument[5]);
            repeat = int.Parse(argument[6]);
            repeat2 = int.Parse(argument[7]);
            growRate = int.Parse(argument[8]);
            explainStringID = argument[9];
        }
    }

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();

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
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.gachaID}");
            }
        }
    }

    public Data GetData(int key)
    {
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return (Data)TableData[key];
    }

    public Dictionary<int, Data> GetDict()
    {
        return TableData.ToDictionary(item => item.Key, item => (Data)item.Value);
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.gachaID, datum);
        }
        TableData = tableData;
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
