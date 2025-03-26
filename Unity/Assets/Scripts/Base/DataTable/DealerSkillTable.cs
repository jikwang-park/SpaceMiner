using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DealerSkillTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public SkillType Type { get; set; }
        public float DamageRatio { get; set; }
        public float CoolTime { get; set; }
        public int MonsterMaxTarget { get; set; }

    }

    private Dictionary<string , Data> dealerDictionary = new Dictionary<string , Data>();


    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var loadHandle = Addressables.LoadAssetAsync<TextAsset>(path);
        loadHandle.WaitForCompletion();

        var list = LoadCsv<Data>(loadHandle.Result.text);


        dealerDictionary.Clear();

        foreach (var item in list)
        {
            if (!dealerDictionary.ContainsKey(item.ID))
            {
                dealerDictionary.Add(item.ID, item);
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
        if (!dealerDictionary.ContainsKey(key))
        {
            return null;
        }
        return dealerDictionary[key];
    }
}
