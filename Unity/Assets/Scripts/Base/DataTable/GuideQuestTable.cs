using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public class GuideQuestTable : DataTable
{
    public enum MissionType
    {
        Exterminate = 1,
        StageClear,
        DungeonClear,
        StatUpgrade,
        Item,
        Building,
    }

    public class Data : ITableData
    {
        public int ID { get; set; }
        public int DetailStringID { get; set; }
        public int Turn { get; set; }
        public MissionType MissionClearType { get; set; }
        public int Target { get; set; }
        public string TargetCount { get; set; }
        public int RewardItemID { get; set; }
        public string RewardItemCount { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            DetailStringID = int.Parse(argument[1]);
            Turn = int.Parse(argument[2]);
            if (int.TryParse(argument[3], out int result))
            {
                MissionClearType = (MissionType)result;
            }
            else
            {
                MissionClearType = System.Enum.Parse<MissionType>(argument[3]);
            }
            Target = int.Parse(argument[4]);
            TargetCount = argument[5];
            RewardItemID = int.Parse(argument[6]);
            RewardItemCount = argument[7];
        }
    }

    public Dictionary<int, Data> orderDict = new Dictionary<int, Data>();

    public List<int> orders = new List<int>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        orderDict.Clear();
        orders.Clear();

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
                orderDict.Add(item.Turn, item);
                orders.Add(item.Turn);
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

    public List<int> GetOrders()
    {
        return orders;
    }

    public Data GetDataByOrder(int questOrder)
    {
        if (!orderDict.ContainsKey(questOrder))
        {
            return null;
        }
        return orderDict[questOrder];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newOrderDict = new Dictionary<int, Data>();
        var newOrders = new List<int>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newOrderDict.Add(datum.Turn, datum);
            newOrders.Add(datum.Turn);
        }
        TableData = tableData;
        orderDict = newOrderDict;
        orders = newOrders;
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
