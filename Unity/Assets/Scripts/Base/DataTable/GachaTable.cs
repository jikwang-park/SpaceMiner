using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaTable : DataTable
{
    public class Data : DataTableData
    {
        public string gachaID { get; set; }
        public string nameStringID { get; set; }
        public string cost_ItemID { get; set; }
        public int cost { get; set; }
        public string cost_Item2ID { get; set; }
        public int cost2 { get; set; }
        public int repeat { get; set; }
        public int repeat2 { get; set; }
        public int growRate { get; set; }
        public string explainStringID { get; set; }

        public override void Set(string[] argument)
        {
            gachaID = argument[0];
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

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        var list = LoadCsv<Data>(text);
        dict.Clear();
        TableData.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.gachaID))
            {
                dict.Add(item.gachaID, item);
                TableData.Add(item.gachaID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.gachaID}");
            }
        }
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
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.gachaID, datum);
            tableData.Add(datum.gachaID, datum);
        }
        dict = dictionary;
        TableData = tableData;
    }


    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
