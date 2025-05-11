using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectItemInventory : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private EffectItemDescribeUI effectItemDescribeUI;

    private List<EffectItemElement> elements = new List<EffectItemElement>();
    private const string prefabFormat = "Prefabs/UI/EffectItemElement";
    private bool isInitialized = false;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        int totalCount = EffectItemInventoryManager.EffectItemInventory.Count;

        foreach (var data in EffectItemInventoryManager.EffectItemInventory)
        {
            var type = data.Key;
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    EffectItemElement inventoryElement = elementObj.GetComponent<EffectItemElement>();
                    inventoryElement.parentInventory = this;
                    inventoryElement.Initialize(type);
                    elements.Add(inventoryElement);

                    if (elements.Count == totalCount)
                    {
                        isInitialized = true;
                        OnClickEffectItem(elements[0]);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
    }
    private void OnEnable()
    {
        if(isInitialized)
        {
            OnClickEffectItem(elements[0]);
        }
    }
    public void OnClickEffectItem(EffectItemElement element)
    {
        effectItemDescribeUI.SetInfo(element.type, element.level);
    }
}
