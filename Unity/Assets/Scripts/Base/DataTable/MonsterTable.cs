using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string Hp { get; set; }
        public string Atk { get; set; }
        public float AtkSpeed { get; set; }
        public float AtkRange { get; set; }
        public float MoveSpeed { get; set; }
        public string MonsterSkill { get; set; }
        public string RewardID { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
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
        var list = LoadCsv<Data>(text);
        dict.Clear();
        TableData.Clear();

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
