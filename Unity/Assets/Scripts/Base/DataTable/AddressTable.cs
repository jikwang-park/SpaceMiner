using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string Address { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Address = argument[1];
        }
    }

    public override System.Type DataType => typeof(Data);

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

    public string GetData(int type)
    {
        if (!TableData.ContainsKey(type))
        {
            return string.Empty;
        }
        return ((Data)TableData[type]).Address;
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
