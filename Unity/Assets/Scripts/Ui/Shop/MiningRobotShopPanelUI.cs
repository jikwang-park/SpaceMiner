using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MiningRobotShopPanelUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    private string prefabFormat = "Prefabs/UI/MiningRobotShopElement";
    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var datas = DataTableManager.ShopTable.GetList(ShopTable.ShopType.MiningRobot);

        foreach (var data in datas)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    MiningRobotShopElement miningRobotShopElement = elementObj.GetComponent<MiningRobotShopElement>();
                    if (miningRobotShopElement != null)
                    {
                        miningRobotShopElement.Initialize(data);
                    }
                }
            };
        }
    }
}
