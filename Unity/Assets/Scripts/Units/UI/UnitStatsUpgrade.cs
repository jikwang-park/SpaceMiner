using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;




public class UnitStatsUpgrade : MonoBehaviour
{
    public UnitStatsUpgradeElement statsUpgradeElements;

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
        //수정해야됌
        var data = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;
        if(data !=null)
        {
            for (int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
            {
                var stats = Instantiate(statsUpgradeElements, parentTransform);
                stats.Init(DataTableManager.UnitUpgradeTable.GetData(1000 * i + 1)); // 디폴트 테이블 추가해달라해야댐
                stats.SetData(data[(UnitUpgradeTable.UpgradeType)i]);
                stats.SetInitString();
            }
        }
    }

}
