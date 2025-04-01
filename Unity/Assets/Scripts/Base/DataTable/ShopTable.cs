using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int StringID { get; set; }
        public int NeedItemID { get; set; }
        public int NeedCount { get; set; }
        public int PaymentItemID { get; set; }
        public int PayCount { get; set; }
        public int DailyPurchaseLimit { get; set; }
        public int ResetTime { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            StringID = int.Parse(argument[1]);
            NeedItemID = int.Parse(argument[2]);
            NeedCount = int.Parse(argument[3]);
            PaymentItemID = int.Parse(argument[4]);
            PayCount = int.Parse(argument[5]);
            DailyPurchaseLimit = int.Parse(argument[6]);
            ResetTime = int.Parse(argument[7]);
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();

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

    public Dictionary<int, Data> GetDict()
    {
        return dict;
    }

    public override void Set(List<string[]> data)
    {
        var newTableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            newTableData.Add(datum.ID, datum);
        }
        TableData = newTableData;
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
