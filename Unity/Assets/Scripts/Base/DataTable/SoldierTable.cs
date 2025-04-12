using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SoldierTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public UnitTypes UnitType { get; set; }
        public Grade Grade { get; set; }
        public int Level { get; set; }
        public float Attack { get; set; }
        public float HP { get; set; }
        public float Defence { get; set; }
        public float Range { get; set; }
        public float MoveSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public int CharacterPrefabID { get; set; }
        public int WeaponPrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NameStringID = int.Parse(argument[1]);
            if (int.TryParse(argument[2], out int kind))
            {
                UnitType = (UnitTypes)kind;
            }
            else
            {
                UnitType = Enum.Parse<UnitTypes>(argument[2]);
            }
            if (int.TryParse(argument[3], out int rating))
            {
                Grade = (Grade)rating;
            }
            else
            {
                Grade = Enum.Parse<Grade>(argument[3]);
            }
            Level = int.Parse(argument[4]);
            Attack = float.Parse(argument[5]);
            HP = float.Parse(argument[6]);
            Defence = float.Parse(argument[7]);
            Range = float.Parse(argument[8]);
            MoveSpeed = float.Parse(argument[9]);
            AttackSpeed = int.Parse(argument[10]);
            CharacterPrefabID = int.Parse(argument[11]);
            WeaponPrefabID = int.Parse(argument[12]);
        }
    }

    private Dictionary<UnitTypes, List<Data>> typeDict = new Dictionary<UnitTypes, List<Data>>();

    public override Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        typeDict.Clear();
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

                if (!typeDict.ContainsKey(item.UnitType))
                {
                    typeDict[item.UnitType] = new List<Data>();
                }
                typeDict[item.UnitType].Add(item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }

        }
    }
    
    public Dictionary<UnitTypes, List<Data>> GetTypeDictionary()
    {
        return typeDict;
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
        var typeDict = new Dictionary<UnitTypes, List<Data>>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            tableData.Add(datum.ID, datum);

            if (!typeDict.ContainsKey(datum.UnitType))
            {
                typeDict[datum.UnitType] = new List<Data>();
            }
            typeDict[datum.UnitType].Add(datum);
        }
        TableData = tableData;
        this.typeDict = typeDict;
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

