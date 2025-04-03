using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class KeyShopPanelUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    private string prefabFormat = "Prefabs/UI/KeyShopElement";

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

        var datas = DataTableManager.ShopTable.GetList(ShopTable.ShopType.DungeonKey);

        foreach (var data in datas)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    KeyShopElement keyShopElement = elementObj.GetComponent<KeyShopElement>();
                    if (keyShopElement != null)
                    {
                        keyShopElement.Initialize(data);
                    }
                }
            };
        }
    }
}
