using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CorpsTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public int FrontSlots { get; set; }
        public string NormalMonsterID { get; set; }
        public int BackSlots { get; set; }
        public string RangedMonsterID { get; set; }
        public string BossMonsterID { get; set; }

        public string[] NormalMonsterIDs;

        public override void Set(string[] argument)
        {
            ID= argument[0];
            FrontSlots = int.Parse(argument[1]);
            NormalMonsterID = argument[2];
            NormalMonsterIDs = NormalMonsterID.Split('_');
            BackSlots = int.Parse(argument[3]);
            RangedMonsterID = argument[4];
            BossMonsterID = argument[5];
        }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        var list = LoadCsv<Data>(text);
        dict.Clear();
        TableData.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.ID))
            {
                item.NormalMonsterIDs = item.NormalMonsterID.Split('_');
                dict.Add(item.ID, item);
                TableData.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }

    public override void Set(List<string[]> data)
    {
        var dictionary  = new Dictionary<string, Data>();
        foreach(var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
        }
        dict = dictionary;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
