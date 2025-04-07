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

    private void Start()
    {
    }

    private void Init()
    {
        for(int i = (int)UnitUpgradeTable.UpgradeType.AttackPoint; i<= (int)UnitUpgradeTable.UpgradeType.CriticalDamages; i++)
        {
            var stats = Instantiate(statsUpgradeElements, parentTransform);
            //stats.SetData();
            stats.Init(DataTableManager.UnitUpgradeTable.GetData(1000*i +1));
        }
    }




    private void Update()
    {
      
    }
}
