using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EffectItemTable : DataTable
{
    public enum ItemType
    {
        Attack = 1,
        HP,
        Defence,
        AttackSpeed,
        BossMonsterDamage,
        NormalMonsterDamage,
        GoldGain,
        ResourceGain,
        MiningRobotProductSpeed,
        MiningRobotMoveSpeed,
        MiningRobotCapacity,
        IdleTime,
    }

    public class Data : ITableData
    {
        public int ID { get; set; }
        public ItemType Type { get; set; }
        public int Level { get; set; }
        public int NeedItemID { get; set; }
        public int NeedItemCount { get; set; }
        public int SpriteID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Type = (ItemType)type;
            }
            else
            {
                Type = Enum.Parse<ItemType>(argument[1]);
            }
            Level = int.Parse(argument[2]);
            NeedItemID = int.Parse(argument[3]);
            NeedItemCount = int.Parse(argument[4]);
            SpriteID = int.Parse(argument[5]);
        }
    }

    private Dictionary<ItemType, List<Data>> typeDict = new Dictionary<ItemType, List<Data>>();

    private Dictionary<int, ItemType> needItemTypeDict = new Dictionary<int, ItemType>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        typeDict.Clear();
        needItemTypeDict.Clear();

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

                if (!typeDict.ContainsKey(item.Type))
                {
                    typeDict.Add(item.Type, new List<Data>());
                }
                typeDict[item.Type].Add(item);

                if (!needItemTypeDict.ContainsKey(item.NeedItemID))
                {
                    needItemTypeDict.Add(item.NeedItemID, item.Type);
                }
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int id)
    {
        if (!TableData.ContainsKey(id))
        {
            throw new ArgumentException("Type Not Found");
        }
        return (Data)TableData[id];
    }

    public List<Data> GetDatas(ItemType type)
    {
        if (!typeDict.ContainsKey(type))
        {
            throw new ArgumentException("Type Not Found");
        }
        return typeDict[type];
    }

    public ItemType GetTypeByID(int itemID)
    {
        if (!needItemTypeDict.ContainsKey(itemID))
        {
            throw new ArgumentException("itemID Not Match");
        }

        return needItemTypeDict[itemID];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newTypeDict = new Dictionary<ItemType, List<Data>>();
        var newNeedItemTypeDict = new Dictionary<int, ItemType>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!newTypeDict.ContainsKey(datum.Type))
            {
                newTypeDict.Add(datum.Type, new List<Data>());
            }
            newTypeDict[datum.Type].Add(datum);
            if (!newNeedItemTypeDict.ContainsKey(datum.NeedItemID))
            {
                newNeedItemTypeDict.Add(datum.NeedItemID, datum.Type);
            }
        }
        TableData = tableData;
        typeDict = newTypeDict;
        needItemTypeDict = newNeedItemTypeDict;
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
