using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDataTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TargetID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Name = argument[1];
            TargetID = int.Parse(argument[2]);
        }
    }

    private Dictionary<string, int> stringDict = new Dictionary<string, int>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        stringDict.Clear();

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

                if (!stringDict.ContainsKey(item.Name))
                {
                    stringDict.Add(item.Name, item.TargetID);
                }
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

    public int GetID(string target)
    {
        if (!stringDict.ContainsKey(target))
        {
            return 0;
        }
        return stringDict[target];
    }

    public override void Set(List<string[]> data)
    {
        var newStringDict = new Dictionary<string, int>();
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            if (!newStringDict.ContainsKey(datum.Name))
            {
                newStringDict.Add(datum.Name, datum.TargetID);
            }
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
