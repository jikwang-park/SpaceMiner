using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TankerSkillTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public SkillType Type { get; set; }
        public float ShieldRatio { get; set; }
        public float Duration { get; set; }
        public float CoolTime {  get; set; }
        public string BuffID { get; set; }
        public string SoilderTarget {  get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = Enum.Parse<SkillType> (argument[1]);
            ShieldRatio = int.Parse(argument[2]);
            Duration = int.Parse(argument[3]);
            CoolTime = int.Parse(argument[4]);
            BuffID = argument[5];
            SoilderTarget = argument[6];
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
        if(!TableData.ContainsKey(key))
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
