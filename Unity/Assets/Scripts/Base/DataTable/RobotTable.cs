using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RobotTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int name { get; set; }
        public Grade grade { get; set; }
        public int miningSpeed { get; set; }
        public int moveSpeed { get; set; }
        public int loadCapacity { get; set; }
        public int IsMergeable { get; set; }
        public string prefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            name = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int grade))
            {
                this.grade = (Grade)grade;
            }
            else
            {
                this.grade = Enum.Parse<Grade>(argument[2]);
            }
            miningSpeed = int.Parse(argument[3]);
            moveSpeed = int.Parse(argument[4]);
            loadCapacity = int.Parse(argument[5]);
            IsMergeable = int.Parse(argument[6]);
            prefabID = argument[7];
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
