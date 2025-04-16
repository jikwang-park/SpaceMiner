using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public int ItemType { get; set; }
        public string MaxStack { get; set; }
        public int SpriteID { get; set; }


        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            ItemType = int.Parse(argument[2]);
            MaxStack = argument[3];
            SpriteID = int.Parse(argument[4]);
        }
    }

    private List<int> ids = new List<int>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        ids.Clear();

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
                ids.Add(item.ID);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
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

    public List<int> GetIds()
    {
        return ids;
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newIds = new List<int>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newIds.Add(datum.ID);
        }
        TableData = tableData;
        ids = newIds;
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
