using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopTable : DataTable
{
    public enum ShopType
    {
        DungeonKey=1,
        Gold,
        MiningRobot,
    }

    public class Data : ITableData
    {
        public int ID { get; set; }
        public int StringID { get; set; }
        public ShopType Type { get; set; }
        public int NeedItemID { get; set; }
        public int NeedCount { get; set; }
        public int PaymentItemID { get; set; }
        public int PayCount { get; set; }
        public int DailyPurchaseLimit { get; set; }
        public int ResetTime { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            StringID = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int type))
            {
                Type = (ShopType)type;
            }
            else
            {
                Type = Enum.Parse<ShopType>(argument[2]);
            }
            NeedItemID = int.Parse(argument[3]);
            NeedCount = int.Parse(argument[4]);
            PaymentItemID = int.Parse(argument[5]);
            PayCount = int.Parse(argument[6]);
            DailyPurchaseLimit = int.Parse(argument[7]);
            ResetTime = int.Parse(argument[8]);
        }
    }

    private Dictionary<int, Data> dict = new Dictionary<int, Data>();

    private Dictionary<ShopType, List<Data>> typeDict = new Dictionary<ShopType, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();
        typeDict.Clear();

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
                dict.Add(item.ID, item);
                if (!typeDict.ContainsKey(item.Type))
                {
                    typeDict.Add(item.Type, new List<Data>());
                }
                typeDict[item.Type].Add(item);
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

    public List<Data> GetList(ShopType type)
    {
        if (typeDict.ContainsKey(type))
        {
            return typeDict[type];
        }
        return null;
    }

    public Dictionary<int, Data> GetDict()
    {
        return dict;
    }

    public override void Set(List<string[]> data)
    {
        var newTableData = new Dictionary<int, ITableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            newTableData.Add(datum.ID, datum);
        }
        TableData = newTableData;
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
