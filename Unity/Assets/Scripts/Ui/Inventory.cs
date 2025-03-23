using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InventoryElement> inventoryElements = new List<InventoryElement>();
    private const string prefabFormat = "Prefabs/UI/InventoryElement";
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private List<Sprite> gradeSprites;

    private InventoryElement selectedElement;
    public void Initialize(List<SoldierTable.Data> dataList)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        inventoryElements.Clear();
        Dictionary<Grade, int> gradeCounters = new Dictionary<Grade, int>();
        int totalCount = dataList.Count;
        int instantiatedCount = 0;

        foreach (var data in dataList)
        {
            int subIndex = 1;
            if (gradeCounters.ContainsKey(data.Rating))
            {
                subIndex = gradeCounters[data.Rating] + 1;
                gradeCounters[data.Rating] = subIndex;
            }
            else
            {
                gradeCounters[data.Rating] = 1;
            }

            var soldierData = data;
            int currentSubIndex = subIndex;

            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    InventoryElement inventoryElement = elementObj.GetComponent<InventoryElement>();
                    Button buttonElement = elementObj.GetComponent<Button>();
                    inventoryElement.parentInventory = this;
                    if (inventoryElement != null)
                    {
                        inventoryElement.SetID(soldierData.ID);
                        inventoryElement.SetGrade(currentSubIndex);
                        inventoryElement.UpdateCount(0);
                        inventoryElement.SetLevel(0);
                        inventoryElement.Deselect();
                        buttonElement.image.sprite = gradeSprites[(int)soldierData.Rating - 1];
                        inventoryElements.Add(inventoryElement);

                    }
                }
                else
                {
                    Debug.LogError("Failed to instantiate prefab with key: " + prefabFormat);
                }

                instantiatedCount++;

                if (instantiatedCount == totalCount && inventoryElements.Count > 0)
                {
                    OnElementSelected(inventoryElements[0]);
                    inventoryElements[0].UnlockElement();
                }
            };
        }
    }
    public void OnElementSelected(InventoryElement element)
    {
        if (selectedElement != null)
        {
            selectedElement.Deselect();
        }
        selectedElement = element;
        selectedElement.Select();
    }
}
