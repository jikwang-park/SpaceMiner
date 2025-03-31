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
        public string PrefabId { get; set; }
        public string Hp { get; set; }
        public string Atk { get; set; }
        public float AtkSpeed { get; set; }
        public float AtkRange { get; set; }
        public float MoveSpeed { get; set; }
        public int MonsterSkill { get; set; }
        public int RewardID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            PrefabId = argument[1];
            Hp = argument[2];
            Atk = argument[3];
            AtkSpeed = float.Parse(argument[4]);
            AtkRange = float.Parse(argument[5]);
            MoveSpeed = float.Parse(argument[6]);
            MonsterSkill = int.Parse(argument[7]);
            RewardID = int.Parse(argument[8]);
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
