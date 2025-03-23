using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CorpsTable : DataTable
{
    public class Data : DataTableData
    {
        public string ID { get; set; }
        public int FrontSlots { get; set; }
        public string NormalMonsterID { get; set; }
        public int BackSlots { get; set; }
        public string RangedMonsterID { get; set; }
        public string BossMonsterID { get; set; }


        public string[] NormalMonsterIDs;
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
                item.NormalMonsterIDs = item.NormalMonsterID.Split('_');
                dict.Add(item.ID, item);
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
        if (!dict.ContainsKey(key))
        {
            return null;
        }
        return dict[key];
    }
}
