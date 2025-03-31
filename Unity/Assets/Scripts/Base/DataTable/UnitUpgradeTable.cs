using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitUpgradeTable : DataTable
{
    public enum UpgradeType
    {
        AttackPoint = 1,
        HealthPoint,
        DefensePoint,
        CriticalPossibility,
        CriticalDamages,
    }

    public class Data : DataTableData
    {
        public int ID { get; set; }
        public UpgradeType Type { get; set; }
        public float Value { get; set; }
        public float StetUpRate { get; set; }
        public float GoldUpRate { get; set; }
        public int Gold { get; set; }
        public float MaxLevel { get; set; }

        public override void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Type = Enum.Parse<UpgradeType>(argument[1]);
            Value = float.Parse(argument[2]);
            StetUpRate = float.Parse(argument[3]);
            GoldUpRate = float.Parse(argument[4]);
            Gold = int.Parse(argument[5]);
            MaxLevel = float.Parse(argument[6]);
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();
    private Dictionary<UpgradeType, Data> dictByType = new Dictionary<UpgradeType, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();
        dictByType.Clear();

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
                TableData.Add(item.ID.ToString(), item);

                if (!dictByType.ContainsKey(item.Type))
                {
                    dictByType.Add(item.Type, item);
                }
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }

    public Data GetData(UpgradeType upgradeType)
    {
        if (!dictByType.ContainsKey(upgradeType))
        {
            return null;
        }
        return dictByType[upgradeType];
    }

    public override void Set(List<string[]> data)
    {
        var dictionary = new Dictionary<int, Data>();
        var typeData = new Dictionary<UpgradeType, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID.ToString(), datum);
            if (!dictByType.ContainsKey(datum.Type))
            {
                dictByType.Add(datum.Type, datum);
            }
        }
        dict = dictionary;
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
