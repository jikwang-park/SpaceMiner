using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialTable : DataTable
{
    public enum QuestTypes
    {
        FirstPlay = 1,
        Gacha,
        SoldierInventory,
        SkillUpgrade,
        Dungeon,
        Building,
        Mine,
        Mining,
    }

    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public int DetailStringID { get; set; }
        public int RewardItemID { get; set; }
        public string RewardItemCount { get; set; }
        public QuestTypes QuestType { get; set; }
        public int QuestNumber { get; set; }
        public int SpriteID { get; set; }


        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            DetailStringID = int.Parse(argument[2]);
            RewardItemID = int.Parse(argument[3]);
            RewardItemCount = argument[4];
            if (int.TryParse(argument[5], out int kind))
            {
                QuestType = (QuestTypes)kind;
            }
            else
            {
                QuestType = Enum.Parse<QuestTypes>(argument[5]);
            }
            QuestNumber = int.Parse(argument[6]);
            SpriteID = int.Parse(argument[7]);
        }
    }

    private Dictionary<QuestTypes, List<Data>> typeDict = new Dictionary<QuestTypes, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
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
                if (!typeDict.ContainsKey(item.QuestType))
                {
                    typeDict.Add(item.QuestType, new List<Data>());
                }
                typeDict[item.QuestType].Add(item);
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

    public List<Data> GetDatas(QuestTypes type)
    {
        if (!typeDict.ContainsKey(type))
        {
            return null;
        }
        return typeDict[type];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newTypeDict = new Dictionary<QuestTypes, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!newTypeDict.ContainsKey(datum.QuestType))
            {
                newTypeDict.Add(datum.QuestType, new List<Data>());
            }
            newTypeDict[datum.QuestType].Add(datum);
        }
        TableData = tableData;
        typeDict = newTypeDict;
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
