using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMergeTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NeedSoldierID { get; set; }
        public int NeedSoldierCount { get; set; }
        public int ResultSoldierID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NeedSoldierID = int.Parse(argument[1]);
            NeedSoldierCount = int.Parse(argument[2]);
            ResultSoldierID = int.Parse(argument[3]);
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
            if (!TableData.ContainsKey(item.ID))
            {
                TableData.Add(item.ID, item);
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

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
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
