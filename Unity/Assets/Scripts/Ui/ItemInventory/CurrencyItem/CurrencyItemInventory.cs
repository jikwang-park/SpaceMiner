using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CurrencyItemInventory : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private CurrencyItemDescribeUI currencyItemDescribeUI;

    private List<CurrencyItemElement> elements = new List<CurrencyItemElement>();
    private const string prefabFormat = "Prefabs/UI/CurrencyItemElement";
    private bool isInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        var currencies = Enum.GetValues(typeof(Currency));
        int totalCount = currencies.Length;

        foreach (var data in currencies)
        {
            var itemId = (int)data;
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    CurrencyItemElement inventoryElement = elementObj.GetComponent<CurrencyItemElement>();
                    inventoryElement.parentInventory = this;
                    inventoryElement.Initialize(itemId);
                    elements.Add(inventoryElement);

                    if (elements.Count == totalCount)
                    {
                        isInitialized = true;
                        OnClickCurrencyItem(elements[0]);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
    }
    public void OnClickCurrencyItem(CurrencyItemElement element)
    {
        currencyItemDescribeUI.SetInfo(element.itemId);
    }
}
