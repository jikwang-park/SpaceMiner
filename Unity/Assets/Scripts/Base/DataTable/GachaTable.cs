using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public int NeedItemID1 { get; set; }
        public int NeedItemCount1 { get; set; }
        public int NeedItemID2 { get; set; }
        public int NeedItemCount2 { get; set; }
        public int RepeatCount1 { get; set; }
        public int RepeatCount2 { get; set; }
        public int DetailStringID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            NeedItemID1 = int.Parse(argument[2]);
            NeedItemCount1 = int.Parse(argument[3]);
            NeedItemID2 = int.Parse(argument[4]);
            NeedItemCount2 = int.Parse(argument[5]);
            RepeatCount1 = int.Parse(argument[6]);
            RepeatCount2 = int.Parse(argument[7]);
            DetailStringID = int.Parse(argument[8]);
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
            if (!TableData.ContainsKey(item.ID))
            {
                TableData.Add(item.ID, item);
                dict.Add(item.ID, item);
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
        var tableData = new Dictionary<int, ITableData>();
        var dict = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            dict.Add(datum.ID, datum);
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
