using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeTable : DataTable
{
    public class Data : ITableData
    {
        public int ID { get; set; }
        public int NameStringID { get; set; }
        public int NeedItemID { get; set; }
        public int NeedItemCount { get; set; }
        public int NeedSkillID { get; set; }
        public int SkillPaymentID { get; set; }

        public void Set(string[] argument)
        {
            ID = int.Parse(argument[0]);
            NeedItemID = int.Parse(argument[1]);
            NameStringID = int.Parse(argument[2]);
            NeedItemCount = int.Parse(argument[3]);
            NeedSkillID = int.Parse(argument[4]);
            SkillPaymentID = int.Parse(argument[5]);
        }
    }

    private Dictionary<int, Data> needSkillIdDict = new Dictionary<int, Data>();

    public override System.Type DataType => typeof(Data);

    public override void LoadFromText(string text)
    {
        TableData.Clear();
        needSkillIdDict.Clear();

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
                if (item.NeedSkillID != 0)
                {
                    needSkillIdDict.Add(item.NeedSkillID, item);
                }
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
    }

    public Data GetData(int needSkillID)
    {
        if (!needSkillIdDict.ContainsKey(needSkillID))
        {
            return null;
        }
        return needSkillIdDict[needSkillID];
    }

    public override void Set(List<string[]> data)
    {
        var newTableData = new Dictionary<int, ITableData>();
        var newNeedSkillIdDict = new Dictionary<int, Data>();
        foreach (var item in data)
        {
            var datum = CreateData<Data>(item);
            newTableData.Add(datum.ID, datum);
            newNeedSkillIdDict.Add(datum.NeedSkillID, datum);
        }
        TableData = newTableData;
        needSkillIdDict = newNeedSkillIdDict;
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
