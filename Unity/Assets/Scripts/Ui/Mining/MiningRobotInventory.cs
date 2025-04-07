using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MiningRobotInventory : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    private string prefabFormat = "Prefabs/UI/MiningRobotSlot";
    private void Start()
    {
        UpdateGridCellSize();
        Initialize();
    }
    private void Initialize()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var datas = MiningRobotInventoryManager.Inventory.slots;

        foreach (var data in datas)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    MiningRobotInventorySlot miningRobotInventorySlot = elementObj.GetComponent<MiningRobotInventorySlot>();
                    if (miningRobotInventorySlot != null)
                    {
                        miningRobotInventorySlot.Initialize(data);
                    }
                }
            };
        }
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
}
