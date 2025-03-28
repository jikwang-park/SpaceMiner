using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string ItemStringID { get; set; }
        public int ItemType { get; set; }
        public int MaxStack { get; set; }
        public string ResourcesName { get; set; }


        public override void Set(string[] argument)
        {
            ID = argument[0];
            ItemStringID = argument[1];
            ItemType = int.Parse(argument[2]);
            MaxStack = int.Parse(argument[3]);
            ResourcesName = argument[4];
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

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
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID, datum);
        }
        dict = dictionary;
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
