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

    private int requireMergeCount = 5;
    private UnitTypes type;
    private InventoryElement selectedElement;
    private InventoryElement currentElement;
    private UnitPartyManager unitPartyManager;
    private SoldierInteractableUI soldierInteractableUI;
    private void Awake()
    {
        unitPartyManager = FindObjectOfType<UnitPartyManager>();
        soldierInteractableUI = GetComponentInChildren<SoldierInteractableUI>();
        soldierInteractableUI.mergeAction += DoMerge;
        soldierInteractableUI.equipAction += Equip;
    }
    private void Start()
    {
        UpdateGridCellSize();
    }

    public void Initialize(List<SoldierTable.Data> dataList, UnitTypes type)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        this.type = type;
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
                    inventoryElements[0].UnlockElement();
                    inventoryElements[0].UpdateCount(9999);
                    OnElementSelected(inventoryElements[0]);
                    Equip();
                }
            };
        }
    }
    public void OnElementSelected(InventoryElement element)
    {
        selectedElement = element;
        var currentIndex = inventoryElements.IndexOf(selectedElement);
        infoMergePanelUI.Initialize(inventoryElements[currentIndex], inventoryElements[currentIndex + 1]);
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
        currentElement = selectedElement;
        currentElement.SetEquip();
        unitPartyManager.SetUnitData(DataTableManager.SoldierTable.GetData(currentElement.soldierId), type);
    }
    private void UnEquip()
    {
        if(currentElement == null)
        {
            return;
        }
        currentElement.SetUnEquip();
        currentElement = null;
    }
    private void Merge(InventoryElement element)
    {
        if(element.IsLocked || element.Count < requireMergeCount)
        {
            Debug.Log("합성 조건에 충족하지 못합니다.");
            return;
        }

        var currentIndex = inventoryElements.IndexOf(element);
        if(currentIndex >= 0 && currentIndex < inventoryElements.Count - 1)
        {
            var newCount = element.Count - requireMergeCount;
            element.UpdateCount(newCount);

            var nextIndex = currentIndex + 1;
            if (inventoryElements[nextIndex].IsLocked)
            {
                inventoryElements[nextIndex].UnlockElement();
                inventoryElements[nextIndex].UpdateCount(1);
            }
            else
            {
                inventoryElements[nextIndex].UpdateCount(inventoryElements[nextIndex].Count + 1);
            }
        }
    }
    public void DoMerge(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Merge(selectedElement);
        }
    }
    public void BatchMerge()
    {
        bool isMerged = true;

        while (isMerged)
        {
            isMerged = false;

            foreach (var element in inventoryElements.ToList())
            {
                while (!element.IsLocked && element.Count >= requireMergeCount)
                {
                    Merge(element);
                    isMerged = true;
                }
            }
        }
        OnElementSelected(selectedElement);
    }
}
