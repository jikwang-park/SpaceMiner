using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CorpsTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int FrontSlots { get; set; }
        public string NormalMonsterID { get; set; }
        public int BackSlots { get; set; }
        public int RangedMonsterID { get; set; }
        public int BossMonsterID { get; set; }

        public int[] NormalMonsterIDs;

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            FrontSlots = int.Parse(argument[1]);
            NormalMonsterID = argument[2];
            NormalMonsterIDs = SplitMonsterId(NormalMonsterID);
            BackSlots = int.Parse(argument[3]);
            RangedMonsterID = int.Parse(argument[4]);
            BossMonsterID = int.Parse(argument[5]);
        }
    }

    public override Type DataType => typeof(Data);

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
                item.NormalMonsterIDs = SplitMonsterId(item.NormalMonsterID);
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

    private static int[] SplitMonsterId(string id)
    {
        string[] idstring = id.Split('_');
        int[] ids = new int[idstring.Length];

        for (int i = 0; i < ids.Length; ++i)
        {
            ids[i] = int.Parse(idstring[i]);
        }

        return ids;
    }
}
