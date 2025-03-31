using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StringTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string Line { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            Line = argument[1];
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

    public string GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return "NULL";
        }
        return dict[key].Line;
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
