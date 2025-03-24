using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public enum Grade
{
    Normal = 1,
    Rare,
    Epic,
    Legend,
}
public class SoldierTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string StringID { get; set; }
        public UnitTypes Kind { get; set; }
        public Grade Rating { get; set; }
        public float Basic_AP { get; set; }
        public float Basic_HP { get; set; }
        public float Basic_DP { get; set; }
        public float Special_DR { get; set; }
        public float Special_CD { get; set; }
        public float Special_H { get; set; }
        public string IncreaseID { get; set; }
        public string BuildingID { get; set; }
        public string SkillID { get; set; }
        public float Distance { get; set; }
        public float MoveSpeed { get; set; }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();
    private Dictionary<UnitTypes, List<Data>> typeDict = new Dictionary<UnitTypes, List<Data>>(); 

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var loadHandle = Addressables.LoadAssetAsync<TextAsset>(path);
        loadHandle.WaitForCompletion();

        var list = LoadCsv<Data>(loadHandle.Result.text);
        dict.Clear();
        typeDict.Clear();

        foreach (var item in list)
        {
            if (!dict.ContainsKey(item.ID))
            {
                dict.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }

            if (!typeDict.ContainsKey(item.Kind))
            {
                typeDict[item.Kind] = new List<Data>();
            }
            typeDict[item.Kind].Add(item);
        }

        Addressables.Release(loadHandle);
    }
    public Dictionary<UnitTypes, List<Data>> GetTypeDictionary()
    {
        return typeDict;
    }
    public Data GetData(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }
}
