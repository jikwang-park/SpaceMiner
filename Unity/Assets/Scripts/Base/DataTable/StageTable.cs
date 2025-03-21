using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class StageTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public string StringID { get; set; }
        public string CorpsID { get; set; }
        public int FirstClearReward { get; set; }
        public int IdleReward { get; set; }
    }

    private Dictionary<string, Data> dict = new Dictionary<string, Data>();

    public override void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);
        var loadHandle = Addressables.LoadAssetAsync<TextAsset>(path);
        loadHandle.WaitForCompletion();

        var list = LoadCsv<Data>(loadHandle.Result.text);
        dict.Clear();

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
        }

        Addressables.Release(loadHandle);
    }

    public Data Get(string key)
    {
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }
}
