using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTable : DataTable
{
    public enum BuildingType
    {
        IdleTime = 1,
        AttackPoint,
        HealthPoint,
        DefensePoint,
        CriticalPossibility,
        CriticalDamages,
        Gold,
        Mining
    }

    public class Data : ITableData
    {
        public int ID { get; set; }
        public BuildingType Type { get; set; }
        public int NameStringID { get; set; }
        public int Level { get; set; }
        public float Value { get; set; }
        public int ItemID { get; set; }
        public int NeedCount { get; set; }
        public int MaxLevel { get; set; }
        public int Sequence { get; set; }
        public string PrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Type = (BuildingType)type;
            }
            else
            {
                Type = System.Enum.Parse<BuildingType>(argument[1]);
            }
            NameStringID = int.Parse(argument[2]);
            Level = int.Parse(argument[3]);
            Value = float.Parse(argument[4]);
            ItemID = int.Parse(argument[5]);
            NeedCount = int.Parse(argument[6]);
            MaxLevel = int.Parse(argument[7]);
            Sequence = int.Parse(argument[8]);
            PrefabID = argument[9];
        }
    }

    private Dictionary<BuildingType, List<Data>> typeDict = new Dictionary<BuildingType, List<Data>>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        typeDict.Clear();

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

                if (!typeDict.ContainsKey(item.Type))
                {
                    typeDict.Add(item.Type, new List<Data>());
                }
                typeDict[item.Type].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public List<Data> GetDatas(BuildingType type)
    {
        if (!typeDict.ContainsKey(type))
        {
            return null;
        }
        return typeDict[type];
    }

    public override void Set(List<string[]> data)
    {
        var tableData = new Dictionary<int, ITableData>();
        var newTypeDict = new Dictionary<BuildingType, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!newTypeDict.ContainsKey(datum.Type))
            {
                newTypeDict.Add(datum.Type, new List<Data>());
            }
            newTypeDict[datum.Type].Add(datum);
        }
        TableData = tableData;
        typeDict = newTypeDict;
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
