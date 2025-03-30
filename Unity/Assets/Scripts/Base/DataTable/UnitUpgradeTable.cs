using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitUpgradeTable : DataTable
{
    public enum UpgradeType
    {
        AttackPoint,
        HealthPoint,
        DefensePoint,
        CriticalPossibility,
        CriticalDamages,
    }

    public class Data : DataTableData
    {
        public string ID { get; set; }
        public UpgradeType Type { get; set; }
        public float AttackUP { get; set; }
        public float HealthUP { get; set; }
        public float DefenseUP { get; set; }
        public float CriticalPUP {  get; set; }
        public float CriticalDUP { get; set; }
        public int Gold { get; set; }
        public float StetUP { get; set; }
        public float GoldUP { get; set; }
        public float MaxLevel { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            Type = Enum.Parse<UpgradeType>(argument[1]);
            AttackUP = float.Parse(argument[2]);
            HealthUP = float.Parse(argument[3]);
            DefenseUP = float.Parse(argument[4]);
            CriticalPUP = float.Parse(argument[5]);
            CriticalDUP = float.Parse(argument[6]);
            Gold = int.Parse(argument[7]);
            StetUP = float.Parse(argument[8]);
            GoldUP= float.Parse(argument[9]);
            MaxLevel = float.Parse(argument[10]);
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
