using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int NeedClearStageID { get; set; }
        public int MiningLevel1 { get; set; }
        public int MiningLevel2 { get; set; }
        public int PrefabID { get; set; }
        public int SpriteID { get; set; }
        public int Distance1 { get; set; }
        public int Distance2 { get; set; }


        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            ItemID = int.Parse(argument[1]);
            NeedClearStageID = int.Parse(argument[2]);
            MiningLevel1 = int.Parse(argument[3]);
            MiningLevel2 = int.Parse(argument[4]);
            PrefabID = int.Parse(argument[5]);
            SpriteID = int.Parse(argument[6]);
            Distance1 = int.Parse(argument[7]);
            Distance2 = int.Parse(argument[8]);
        }
    }

    private List<int> idList = new List<int>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        idList.Clear();

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
                idList.Add(item.ID);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public List<int> GetIds()
    {
        return idList;
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
        var newIdList = new List<int>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newIdList.Add(datum.ID);
        }
        TableData = tableData;
        idList = newIdList;
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
