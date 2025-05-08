using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DealerSkillTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public Grade Grade { get; set; }
        public float DamageRatio { get; set; }
        public float CoolTime { get; set; }
        public int MonsterMaxTarget { get; set; }
        public int Level { get; set; }
        public int NameStringID { get; set; }
        public int DetailStringID { get; set; }
        public int PrefabID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Grade = (Grade)type;
            }
            else
            {
                Grade = Enum.Parse<Grade>(argument[1]);
            }
            DamageRatio = float.Parse(argument[2]);
            CoolTime = float.Parse(argument[3]);
            MonsterMaxTarget = int.Parse(argument[4]);
            Level = int.Parse(argument[5]);
            NameStringID = int.Parse(argument[6]);
            DetailStringID = int.Parse(argument[7]);
            PrefabID = int.Parse(argument[8]);
        }
    }

    private Dictionary<Grade, int> gradeMaxLevel = new Dictionary<Grade, int>();

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

                if (!gradeMaxLevel.ContainsKey(item.Grade))
                {
                    gradeMaxLevel.Add(item.Grade, item.Level);
                }
                if (gradeMaxLevel[item.Grade] < item.Level)
                {
                    gradeMaxLevel[item.Grade] = item.Level;
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

    public int GetMaxLevel(Grade grade)
    {
        return gradeMaxLevel[grade];
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
