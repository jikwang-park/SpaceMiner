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
            Instantiate(statsUpgradeElements[i],parentTransform);
            statsUpgradeElements[i].SetData(i + 100);
        }
    }




    private void Update()
    {
      
    }
}
