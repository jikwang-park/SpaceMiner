using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class VoucherItemInventory : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private VoucherItemDescribeUI currencyItemDescribeUI;

    private List<VoucherItemElement> elements = new List<VoucherItemElement>();
    private const string prefabFormat = "Prefabs/UI/VoucherItemElement";
    private bool isInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        var itemIds = DataTableManager.ItemTable.GetIds();
        var vouchers = itemIds.Where((e) => DataTableManager.ItemTable.GetData(e).ItemType == 5 || DataTableManager.ItemTable.GetData(e).ItemType == 6).ToList();
        int totalCount = vouchers.Count;

        foreach (var data in vouchers)
        {
            var itemId = data;
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    VoucherItemElement inventoryElement = elementObj.GetComponent<VoucherItemElement>();
                    inventoryElement.parentInventory = this;
                    inventoryElement.Initialize(itemId);
                    elements.Add(inventoryElement);

                    if (elements.Count == totalCount)
                    {
                        isInitialized = true;
                        OnClickVoucherItem(elements[0]);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
    }
    public void OnClickVoucherItem(VoucherItemElement element)
    {
        currencyItemDescribeUI.SetInfo(element.itemId);
    }
}
