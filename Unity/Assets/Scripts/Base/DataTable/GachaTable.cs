using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaTable : DataTable
{
    public class Data : ITableData
    {
        public int gachaID { get; set; }
        public int nameStringID { get; set; }
        public int cost_ItemID { get; set; }
        public int cost { get; set; }
        public int cost_Item2ID { get; set; }
        public int cost2 { get; set; }
        public int repeat { get; set; }
        public int repeat2 { get; set; }
        public int growRate { get; set; }
        public int explainStringID { get; set; }

        public void Set(string[] argument)
        {
            gachaID = int.Parse(argument[0]);
            nameStringID = int.Parse(argument[1]);
            cost_ItemID = int.Parse(argument[2]);
            cost = int.Parse(argument[3]);
            cost_Item2ID = int.Parse(argument[4]);
            cost2 = int.Parse(argument[5]);
            repeat = int.Parse(argument[6]);
            repeat2 = int.Parse(argument[7]);
            growRate = int.Parse(argument[8]);
            explainStringID = int.Parse(argument[9]);
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        dict.Clear();

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
                dict.Add(item.gachaID, item);
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
        return dict;
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var dict = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.gachaID, datum);
            dict.Add(datum.gachaID, datum);
        }
        TableData = tableData;
        this.dict = dict;
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
