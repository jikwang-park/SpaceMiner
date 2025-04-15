using System;
using System.Collections;
using System.Collections.Generic;
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

    public class Data : ITableData
    {
        public int ID { get; set; }
        public UpgradeType Type { get; set; }
        public float Value { get; set; }
        public int NeedItemID { get; set; }
        public int NeedItemCount { get; set; }
        public int MaxLevel { get; set; }
        public int NameStringID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Type = (UpgradeType)type;
            }
            else
            {
                Type = Enum.Parse<UpgradeType>(argument[1]);
            }
            Value = float.Parse(argument[2]);
            NeedItemID = int.Parse(argument[3]);
            NeedItemCount = int.Parse(argument[4]);
            MaxLevel = int.Parse(argument[5]);
            NameStringID = int.Parse(argument[6]);
        }
    }

    private Dictionary<UpgradeType, Data> dictByType = new Dictionary<UpgradeType, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        dictByType.Clear();

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
        if (!TableData.ContainsKey(key))
        {
            return null;
        }
        return (Data)TableData[key];
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
        var typeData = new Dictionary<UpgradeType, Data>();
        var tableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            if (!dictByType.ContainsKey(datum.Type))
            {
                dictByType.Add(datum.Type, datum);
            }
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
