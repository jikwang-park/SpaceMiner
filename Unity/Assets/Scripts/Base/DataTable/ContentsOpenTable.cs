using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsOpenTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int TargetStageID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            TargetStageID = int.Parse(argument[1]);
        }
    }

    private List<int> ids = new List<int>();
    public override System.Type DataType => typeof(Data);

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

    public int GetData(int type)
    {
        if (!TableData.ContainsKey(type))
        {
            throw new System.ArgumentException("Key Not Found");
        }
        return ((Data)TableData[type]).TargetStageID;
    }

    public List<int> GetIds()
    {
        return ids;
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
