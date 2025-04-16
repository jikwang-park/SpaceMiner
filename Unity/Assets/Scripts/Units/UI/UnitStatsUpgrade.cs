using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;




public class UnitStatsUpgrade : MonoBehaviour
{
    public UnitStatsUpgradeElement statsUpgradeElements;

    private const string prefabFormat = "Prefabs/UI/Stats";
    

    [SerializeField]
    private Transform parentTransform;

    [SerializeField]
    private List<Sprite> statsSprite;


    private void Awake()
    {
        Init();
    }
    private void Init()
    {

        var data = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;
        for (int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
        {
            var currentType = (UnitUpgradeTable.UpgradeType)i;
            Addressables.InstantiateAsync(prefabFormat, parentTransform).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {

                    GameObject element = handle.Result;
                    UnitStatsUpgradeElement statsElement = element.GetComponent<UnitStatsUpgradeElement>();
                    if (statsElement != null)
                    {
                        statsElement.SetData(data[currentType], DataTableManager.UnitUpgradeTable.GetData(currentType));
                        statsElement.SetInitString(currentType);
                        statsElement.SetImage(currentType, statsSprite);
                    }

                }
                else
                {
                    Debug.LogError("Failed Key address " + prefabFormat);
                }
            };
        };
        //if (data !=null)
        //{
        //    for (int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
        //    {
        //        var stats = Instantiate(statsUpgradeElements, parentTransform);
        //        stats.Init(DataTableManager.UnitUpgradeTable.GetData(1000 * i + 1)); // 디폴트 테이블 추가해달라해야댐
        //        stats.SetData(data[(UnitUpgradeTable.UpgradeType)i]);
        //        stats.SetInitString();
        //    }
        //}

    }
}
