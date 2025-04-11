using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField]
    private InfoMergePanelUI infoMergePanelUI;
    [SerializeField]
    private SoldierInteractableUI soldierInteractableUI; 

    private UnitTypes type;
    private InventoryElement selectedElement;
    private InventoryElement equipElement;
    private UnitPartyManager unitPartyManager;
    public void Initialize(UnitTypes type)
    {
        unitPartyManager = FindObjectOfType<UnitPartyManager>();
        soldierInteractableUI.equipAction += Equip;
        InitializeInventory(type);
    }
    private void OnDisable()
    {
        OnElementSelected(equipElement);
    }
    private void InitializeInventory(UnitTypes type)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        this.type = type;
        var datas = InventoryManager.GetInventoryData(this.type);
        inventoryElements.Clear();
        Dictionary<Grade, int> gradeCounters = new Dictionary<Grade, int>();
        int totalCount = datas.elements.Count;
        int instantiatedCount = 0;

        foreach (var data in datas.elements)
        {
            int subIndex = 1;
            if (gradeCounters.ContainsKey(data.grade))
            {
                subIndex = gradeCounters[data.grade] + 1;
                gradeCounters[data.grade] = subIndex;
            }
            else
            {
                gradeCounters[data.grade] = 1;
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
                        if(!soldierData.isLocked)
                        {
                            inventoryElement.UnlockElement();
                        }
                        inventoryElement.SetID(soldierData.soldierId);
                        inventoryElement.SetGrade(soldierData.grade);
                        inventoryElement.UpdateCount(soldierData.count);
                        inventoryElement.SetLevel(currentSubIndex);
                        buttonElement.image.sprite = gradeSprites[(int)soldierData.grade - 1];
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
                    var equipElement = inventoryElements.Find((e) => e.soldierId == datas.equipElementID);
                    OnElementSelected(equipElement);
                    Equip();
                    OnElementSelected(inventoryElements[0]);
                }
            };
        }
    }
    public void OnElementSelected(InventoryElement element)
    {
        selectedElement = element;
        var currentIndex = inventoryElements.IndexOf(selectedElement);
        if(currentIndex < inventoryElements.Count)
        {
            infoMergePanelUI.Initialize(inventoryElements[currentIndex], inventoryElements[currentIndex + 1]);
        }
        selectedElement.Select();
    }
    public void UpdateGridCellSize()
    {
        GridLayoutGroup gridLayout = contentParent.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            return;
        }
        RectTransform rectTransform = contentParent.GetComponent<RectTransform>();
        float totalWidth = rectTransform.rect.width;
        int columns = gridLayout.constraintCount;
        int leftPadding = gridLayout.padding.left;
        int rightPadding = gridLayout.padding.right;
        float spacingX = gridLayout.spacing.x;

        float availableWidth = totalWidth - leftPadding - rightPadding - spacingX * (columns - 1);
        float cellSize = availableWidth / columns;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
    private void Equip()
    {
        UnEquip();
        equipElement = selectedElement;
        equipElement.SetEquip();
        InventoryManager.Equip(type, equipElement.soldierId);
        unitPartyManager.SetUnitData(DataTableManager.SoldierTable.GetData(equipElement.soldierId), type);
    }
    private void UnEquip()
    {
        if(equipElement == null)
        {
            return;
        }
        equipElement.SetUnEquip();
        equipElement = null;
    }
    public void BatchMerge()
    {
        bool isMerged = true;

        while (isMerged)
        {
            isMerged = false;

            foreach (var element in inventoryElements.ToList())
            {
                while (!element.IsLocked && element.Count >= InventoryManager.requireMergeCount)
                {
                    InventoryManager.Merge(element.soldierId);
                    isMerged = true;
                }
            }
        }
        OnElementSelected(selectedElement);
        SaveLoadManager.SaveGame();
    }
    public void ApplyChangesInventory()
    {
        var datas = InventoryManager.GetInventoryData(this.type).elements;

        for(int i = 0; i < datas.Count; i++)
        {
            if (inventoryElements[i].IsLocked && !datas[i].isLocked)
            {
                inventoryElements[i].UnlockElement();
            }
            inventoryElements[i].UpdateCount(datas[i].count);
        }
    }
}
