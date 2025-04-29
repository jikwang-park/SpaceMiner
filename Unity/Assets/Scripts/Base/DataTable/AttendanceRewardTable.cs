using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendanceRewardTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int AttendanceID { get; set; }
        public int Day { get; set; }
        public int RewardItemID { get; set; }
        public string RewardItemCount { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            AttendanceID = int.Parse(argument[1]);
            Day = int.Parse(argument[2]);
            RewardItemID = int.Parse(argument[3]);
            RewardItemCount = argument[4];
        }
    }

    private Dictionary<int, Dictionary<int, Data>> rewardDict = new Dictionary<int, Dictionary<int, Data>>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        rewardDict.Clear();

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

                if (!rewardDict.ContainsKey(item.AttendanceID))
                {
                    rewardDict.Add(item.AttendanceID, new Dictionary<int, Data>());
                }
                rewardDict[item.AttendanceID].Add(item.Day, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int attendenceID, int day)
    {
        if (!rewardDict.ContainsKey(attendenceID)
            || !rewardDict[attendenceID].ContainsKey(day))
        {
            return null;
        }
        return rewardDict[attendenceID][day];
    }

    public override void Set(List<string[]> data)
    {
        var newRewardDict = new Dictionary<int, Dictionary<int, Data>>();
        var tableData = new Dictionary<int, ITableData>();

        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            if (!newRewardDict.ContainsKey(datum.AttendanceID))
            {
                newRewardDict.Add(datum.AttendanceID, new Dictionary<int, Data>());
            }
            newRewardDict[datum.AttendanceID].Add(datum.Day, datum);
        }
        TableData = tableData;
        rewardDict = newRewardDict;
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
