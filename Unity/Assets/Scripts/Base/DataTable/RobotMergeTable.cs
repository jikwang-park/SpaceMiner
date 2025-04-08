using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RobotMergeTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int materialRobotID { get; set; }
        public int resultID { get; set; }
        public int probability { get; set; }


        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            materialRobotID = int.Parse(argument[1]);
            resultID = int.Parse(argument[2]);
            probability = int.Parse(argument[3]);
        }
    }

    public Dictionary<int, Data> materialDict = new Dictionary<int, Data>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        materialDict.Clear();

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
                materialDict.Add(item.materialRobotID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int robotID)
    {
        if (!materialDict.ContainsKey(robotID))
        {
            return null;
        }
        return materialDict[robotID];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newMaterialDict = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);
            newMaterialDict.Add(datum.materialRobotID, datum);
        }
        TableData = tableData;
        materialDict = newMaterialDict;
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
