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
        public int StringID { get; set; }
        public int Turn { get; set; }
        public MissionType MissionClearType { get; set; }
        public int Target { get; set; }
        public int RewardID { get; set; }
        public int RewardCount { get; set; }
        public string Prefab { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            StringID = int.Parse(argument[1]);
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
            RewardID = int.Parse(argument[5]);
            RewardCount = int.Parse(argument[6]);
            Prefab = argument[7];
        }
    }


    public override System.Type DataType => typeof(Data);

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
