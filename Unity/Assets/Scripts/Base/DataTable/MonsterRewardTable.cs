using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterRewardTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string Reward1 { get; set; }
        public int Count { get; set; }
        public string Reward2 { get; set; }
        public string CountArray { get; set; }
        public string Probability { get; set; }

        public int[] counts;
        public float[] probabilities;

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            Reward1 = argument[1];
            Count = int.Parse(argument[2]);
            Reward2 = argument[3];
            CountArray = argument[4];
            Probability = argument[5];
        }

        public int RandomReward2()
        {
            if (Reward2 == "0")
            {
                return -1;
            }

            float random = Random.value;

            float currentProb = probabilities[0];
            int index = 0;

            while (currentProb < random && index - 1 < probabilities.Length)
            {
                ++index;
                currentProb += probabilities[index];
            }

            return index;
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
                TableData.Add(item.ID, item);
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
