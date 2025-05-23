using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TankerSkillTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public Grade Grade { get; set; }
        public float ShieldRatio { get; set; }
        public float Duration { get; set; }
        public float CoolTime { get; set; }
        public int BuffID { get; set; }
        public string SoldierTarget { get; set; }
        public int Level { get; set; }
        public int NameStringID { get; set; }
        public int DetailStringID { get; set; }
        public int MaxLevel { get; set; }
        public int SpriteID { get; set; }


        //public UnitTypes[] targetPriority;

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
            ShieldRatio = int.Parse(argument[2]);
            Duration = int.Parse(argument[3]);
            CoolTime = int.Parse(argument[4]);
            BuffID = int.Parse(argument[5]);
            SoldierTarget = argument[6];
            Level = int.Parse(argument[7]);
            NameStringID = int.Parse(argument[8]);
            DetailStringID = int.Parse(argument[9]);
            MaxLevel = int.Parse(argument[10]);
            SpriteID = int.Parse(argument[11]);

            //targetPriority = SplitSoldierTarget(SoldierTarget);
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


    //private static UnitTypes[] SplitSoldierTarget(string id)
    //{
    //    string[] idstring = id.Split('_');
    //    UnitTypes[] ids = new UnitTypes[idstring.Length];

    //    for (int i = 0; i < ids.Length; ++i)
    //    {
    //        if (int.TryParse(idstring[i], out int result))
    //        {
    //            ids[i] = (UnitTypes)result;
    //        }
    //        else
    //        {
    //            ids[i] = Enum.Parse<UnitTypes>(idstring[i]);
    //        }
    //    }

    //    return ids;
    //}
}
