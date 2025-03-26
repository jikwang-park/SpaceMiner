using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HealerSkillTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public SkillType Type { get; set; }
        public float HealRatio { get; set; }
        public float CoolTime { get; set; }
        public string BuffID { get; set; }
        public string SoilderTarget { get; set; }

    }
    private Dictionary<string, Data> HealerDictionary = new Dictionary<string, Data>();

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var loadHandle = Addressables.LoadAssetAsync<TextAsset>(path);
        loadHandle.WaitForCompletion();

        var list = LoadCsv<Data>(loadHandle.Result.text);


        HealerDictionary.Clear();

        foreach (var item in list)
        {
            if (!HealerDictionary.ContainsKey(item.ID))
            {
                HealerDictionary.Add(item.ID, item);
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
        if (!HealerDictionary.ContainsKey(key))
        {
            return null;
        }
        return HealerDictionary[key];
    }
}
