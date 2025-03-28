using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterRewardTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string Reward1 { get; set; }
        public int Count { get; set; }
        public string Reward2 { get; set; }
        public string CountArray { get; set; }
        public string Probability { get; set; }

        public int[] counts;
        public float[] probabilities;

        public override void Set(string[] argument)
        {
            ID = argument[0];
            Reward1 = argument[1];
            Count = int.Parse(argument[2]);
            Reward2 = argument[3];
            CountArray = argument[4];
            Probability = argument[5];
        }
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
                var strCounts = item.CountArray.Split('_');
                int countLength = strCounts.Length;
                item.counts = new int[countLength];
                for (int i = 0; i < countLength; ++i)
                {
                    item.counts[i] = int.Parse(strCounts[i]);
                }

                var strProbabilities = item.Probability.Split('_');
                countLength = strProbabilities.Length;
                item.probabilities = new float[countLength];
                for (int i = 0; i < countLength; ++i)
                {
                    item.probabilities[i] = float.Parse(strProbabilities[i]);
                }

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
