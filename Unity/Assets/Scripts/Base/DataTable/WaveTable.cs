using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WaveTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public string CorpsID { get; set; }

        public int[] CorpsIDs;

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            CorpsID = argument[1];
            CorpsIDs = SplitWaveCorpsID(CorpsID);
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
                item.CorpsIDs = SplitWaveCorpsID(item.CorpsID);
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
        return TableData[key] as Data;
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

    private static int[] SplitWaveCorpsID(string waveCorpsID)
    {
        var waveCorpsIDs = waveCorpsID.Split("_");
        var waveCorpsIntIDs = new int[waveCorpsIDs.Length];

        for (int i = 0; i < waveCorpsIntIDs.Length; ++i)
        {
            waveCorpsIntIDs[i] = int.Parse(waveCorpsIDs[i]);
        }
        return waveCorpsIntIDs;
    }
}
