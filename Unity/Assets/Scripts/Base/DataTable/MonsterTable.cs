using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string Hp { get; set; }
        public string Atk { get; set; }
        public float AtkSpeed { get; set; }
        public float AtkRange { get; set; }
        public float MoveSpeed { get; set; }
        public string MonsterSkill { get; set; }
        public string RewardID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Hp = argument[1];
            Atk = argument[2];
            AtkSpeed = float.Parse(argument[3]);
            AtkRange = float.Parse(argument[4]);
            MoveSpeed = float.Parse(argument[5]);
            MonsterSkill = argument[6];
            RewardID = argument[7];
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

    public Data GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
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
