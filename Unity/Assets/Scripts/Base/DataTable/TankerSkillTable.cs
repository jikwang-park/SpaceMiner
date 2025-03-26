using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;




public class TankerSkillTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public SkillType Type { get; set; }

        public float ShieldRatio { get; set; }
        public float Duration { get; set; }
        public float CoolTime {  get; set; }
        public string BuffID { get; set; }
        public string SoilderTarget {  get; set; }

    }

    private Dictionary<string, Data> tankerDictionary = new Dictionary<string, Data>();



    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var loadHandle = Addressables.LoadAssetAsync<TextAsset>(path);
        loadHandle.WaitForCompletion();

        var list = LoadCsv<Data>(loadHandle.Result.text);


        tankerDictionary.Clear();

        foreach (var item in list)
        {
            if (!tankerDictionary.ContainsKey(item.ID))
            {
                tankerDictionary.Add(item.ID, item);
            }
            else
            {
                Debug.Log($"Key Duplicated: {item.ID}");
            }
        }
        Addressables.Release(loadHandle);

    }

    public Data GetData(string key)
    {
        if(!tankerDictionary.ContainsKey(key))
        {
            return null;
        }
        return tankerDictionary[key];
    }
}
