using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectItemInventory : MonoBehaviour
{
    private const string prefabFormat = "Prefabs/UI/EffectItemElement";
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private EffectItemDescribeUI effectItemDescribeUI;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
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
                    if (inventoryElement != null)
                    {
                        inventoryElement.Initialize(type);
                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }
            };
        }
    }
    public void OnClickEffectItem(EffectItemTable.ItemType type, int level)
    {
        effectItemDescribeUI.SetInfo(type, level);
    }
}
