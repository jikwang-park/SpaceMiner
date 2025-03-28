using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HealerSkillTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public SkillType Type { get; set; }
        public float HealRatio { get; set; }
        public float CoolTime { get; set; }
        public string BuffID { get; set; }
        public string SoilderTarget { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            Type = Enum.Parse<SkillType>(argument[1]);
            HealRatio = float.Parse(argument[2]);
            CoolTime = float.Parse(argument[3]);
            BuffID = argument[4];
            SoilderTarget = argument[5];
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
        var dictionary = new Dictionary<string, Data>();
        var tableData = new Dictionary<string, DataTableData>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            dictionary.Add(datum.ID, datum);
            tableData.Add(datum.ID, datum);
        }
        dict = dictionary;
        TableData = tableData;
    }

    public override string GetCsvData()
    {
        return CreateCsv(dict.Values.ToList());
    }
}
