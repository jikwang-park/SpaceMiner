using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HealerSkillTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public Grade Type { get; set; }
        public float HealRatio { get; set; }
        public float CoolTime { get; set; }
        public int BuffID { get; set; }
        public string SoldierTarget { get; set; }
        public int Level { get; set; }

        public UnitTypes[] targetPriority;

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            if (int.TryParse(argument[1], out int type))
            {
                Type = (Grade)type;
            }
            else
            {
                Type = Enum.Parse<Grade>(argument[1]);
            }
            HealRatio = float.Parse(argument[2]);
            CoolTime = float.Parse(argument[3]);
            BuffID = int.Parse(argument[4]);
            SoldierTarget = argument[5];
            Level = int.Parse(argument[6]);

            targetPriority = SplitSoldierTarget(SoldierTarget);
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
                item.targetPriority = SplitSoldierTarget(item.SoldierTarget);
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

    private static UnitTypes[] SplitSoldierTarget(string id)
    {
        string[] idstring = id.Split('_');
        UnitTypes[] ids = new UnitTypes[idstring.Length];

        for (int i = 0; i < ids.Length; ++i)
        {
            if (int.TryParse(idstring[i], out int result))
            {
                ids[i] = (UnitTypes)result;
            }
            else
            {
                ids[i] = Enum.Parse<UnitTypes>(idstring[i]);
            }
        }

        return ids;
    }
}
