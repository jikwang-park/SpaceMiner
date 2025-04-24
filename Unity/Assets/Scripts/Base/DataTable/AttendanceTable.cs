using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int NameStringID { get; set; }
        public int DetailStringID { get; set; }
        public int Period { get; set; }
        public int IsRepeat { get; set; }
        public int SpriteID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = int.Parse(argument[1]);
            NameStringID = int.Parse(argument[2]);
            DetailStringID = int.Parse(argument[3]);
            Period = int.Parse(argument[4]);
            IsRepeat = int.Parse(argument[5]);
            SpriteID = int.Parse(argument[6]);
        }
    }

    private List<Data> dataList = new List<Data>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        dataList.Clear();

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
                dataList.Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public List<Data> GetList()
    {
        return dataList;
    }

    public override void Set(List<string[]> data)
    {
        var newList = new List<Data>();
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newList.Add(datum);
        }
        TableData = tableData;
        dataList = newList;
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
