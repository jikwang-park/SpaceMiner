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
        public int NameStringID { get; set; }
        public Grade Grade { get; set; }
        public int MiningSpeed { get; set; }
        public int MoveSpeed { get; set; }
        public int ProductCapacity { get; set; }
        public int SpriteID { get; set; }
        public int PrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int grade))
            {
                this.Grade = (Grade)grade;
            }
            else
            {
                this.Grade = Enum.Parse<Grade>(argument[2]);
            }
            MiningSpeed = int.Parse(argument[3]);
            MoveSpeed = int.Parse(argument[4]);
            ProductCapacity = int.Parse(argument[5]);
            SpriteID = int.Parse(argument[6]);
            PrefabID = int.Parse(argument[7]);
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
