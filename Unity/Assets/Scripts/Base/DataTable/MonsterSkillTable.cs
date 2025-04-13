using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSkillTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int PrefabID { get; set; }
        public float AttackRatio { get; set; }
        public float SkillRange { get; set; }
        public float CoolTime { get; set; }
        public int MaxTargetCount { get; set; }
        public TargetPriority TargetPriority { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            PrefabID = int.Parse(argument[1]);
            AttackRatio = float.Parse(argument[2]);
            SkillRange = float.Parse(argument[3]);
            CoolTime = float.Parse(argument[4]);
            MaxTargetCount = int.Parse(argument[5]);
            if (int.TryParse(argument[6], out int type))
            {
                TargetPriority = (TargetPriority)type;
            }
            else
            {
                TargetPriority = Enum.Parse<TargetPriority>(argument[6]);
            }
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