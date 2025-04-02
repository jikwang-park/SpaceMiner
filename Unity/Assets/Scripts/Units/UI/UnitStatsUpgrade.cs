using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;




public class UnitStatsUpgrade : MonoBehaviour
{
    public List<UnitStatsUpgradeElement> statsUpgradeElements = new List<UnitStatsUpgradeElement>();

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
        for(int i = 0;  i< statsUpgradeElements.Count; i++)
        {
            var stats = Instantiate(statsUpgradeElements[i],parentTransform);
            stats.Init(DataTableManager.UnitUpgradeTable.GetData(1000*(1+i) +1));
        }
    }




    private void Update()
    {
      
    }
}
