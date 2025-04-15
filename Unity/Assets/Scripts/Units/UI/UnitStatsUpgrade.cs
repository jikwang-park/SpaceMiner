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
        //�����ؾ߉�
        var data = SaveLoadManager.Data.unitStatUpgradeData.upgradeLevels;
        if(data !=null)
        {
            for (int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i <= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
            {
                var stats = Instantiate(statsUpgradeElements, parentTransform);
                stats.Init(DataTableManager.UnitUpgradeTable.GetData(1000 * i + 1)); // ����Ʈ ���̺� �߰��ش޶��ؾߴ�
                stats.SetData(data[(UnitUpgradeTable.UpgradeType)i]);
                stats.SetInitString();
            }
        }
    }

}
