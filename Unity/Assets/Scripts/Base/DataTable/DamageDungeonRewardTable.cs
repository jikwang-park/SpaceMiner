using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDungeonRewardTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string NeedDamage { get; set; }
        public int RewardItemID { get; set; }
        public string RewardItemCount { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NeedDamage = argument[1];
            RewardItemID = int.Parse(argument[2]);
            RewardItemCount = argument[3];
        }
    }

    private List<Data> dataList = new List<Data>();
    private SortedList<int, BigNumber> totalReward = new SortedList<int, BigNumber>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        dataList.Clear();
        totalReward.Clear();
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
                dataList.Add(item);
                if (!totalReward.ContainsKey(item.RewardItemID))
                {
                    totalReward.Add(item.RewardItemID, 0);
                }
                totalReward[item.RewardItemID] += item.RewardItemCount;
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(BigNumber damage)
    {
        for (int i = 1; i < dataList.Count; ++i)
        {
            if (dataList[i].NeedDamage > damage && dataList[i - 1].NeedDamage < damage)
            {
                return dataList[i - 1];
            }
        }

        if (damage > dataList[dataList.Count - 1].NeedDamage)
        {
            return dataList[dataList.Count - 1];
        }

        return null;
    }

    public SortedList<int, BigNumber> GetRewards(BigNumber damage)
    {
        SortedList<int, BigNumber> rewards = new SortedList<int, BigNumber>();
        for (int i = 0; i < dataList.Count; ++i)
        {
            if (dataList[i].NeedDamage > damage)
            {
                continue;
            }
            if (!rewards.ContainsKey(dataList[i].RewardItemID))
            {
                rewards.Add(dataList[i].RewardItemID, 0);
            }
            rewards[dataList[i].RewardItemID] += dataList[i].RewardItemCount;
        }
        return rewards;
    }

    public SortedList<int, BigNumber> GetRewards()
    {
        return totalReward;
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newDataList = new List<Data>();
        var newTotalReward = new SortedList<int, BigNumber>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newDataList.Add(datum);
            if (!newTotalReward.ContainsKey(datum.RewardItemID))
            {
                newTotalReward.Add(datum.RewardItemID, 0);
            }
            newTotalReward[datum.RewardItemID] += datum.RewardItemCount;
        }
        TableData = tableData;
        dataList = newDataList;
        totalReward = newTotalReward;
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
