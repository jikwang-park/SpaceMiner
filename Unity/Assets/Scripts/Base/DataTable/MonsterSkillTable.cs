using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSkillTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string StringID { get; set; }
        public int AtkRatio { get; set; }
        public float SkillRange { get; set; }
        public float CoolTime { get; set; }
        public int MaxCount { get; set; }
        public TargetPriority Type { get; set; }

        public override void Set(string[] argument)
        {
            ID = argument[0];
            StringID = argument[1];
            AtkRatio = int.Parse(argument[2]);
            SkillRange = float.Parse(argument[3]);
            CoolTime = float.Parse(argument[4]);
            MaxCount = int.Parse(argument[5]);
            Type = Enum.Parse<TargetPriority>(argument[6]);
        }
    }
    public enum TargetPriority
    {
        /// <summary>
        /// ÅÊÄ¿-µô·¯-Èú·¯
        /// </summary>
        FrontOrder = 1,
        /// <summary>
        /// Èú·¯-µô·¯-ÅÊÄ¿
        /// </summary>
        BackOrder = 2,
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        dict.Clear();
        TableData.Clear();

        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var list = LoadCsv<Data>(text);

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