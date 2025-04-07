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
    private List<MiningRobotInventorySlot> miningRobotInventorySlots = new List<MiningRobotInventorySlot>();

    private string prefabFormat = "Prefabs/UI/MiningRobotSlot";
    private void Awake()
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

        for(int i = 0; i < datas.Count; i++)
        {
            int index = i;
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    MiningRobotInventorySlot miningRobotInventorySlot = elementObj.GetComponent<MiningRobotInventorySlot>();
                    if (miningRobotInventorySlot != null)
                    {
                        miningRobotInventorySlot.Initialize(datas[index]);
                        miningRobotInventorySlot.index = index;
                        miningRobotInventorySlots.Add(miningRobotInventorySlot);
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
    private void OnEnable()
    {
        MiningRobotInventoryManager.onChangedInventory += DoInventoryChanged;
        UpdateUI();
    }
    private void OnDisable()
    {
        MiningRobotInventoryManager.onChangedInventory -= DoInventoryChanged;
    }
    private void DoInventoryChanged(int index, MiningRobotInventorySlotData data)
    {
        miningRobotInventorySlots[index].Initialize(data);
    }
    private void UpdateUI()
    {
        var datas = MiningRobotInventoryManager.Inventory.slots;
        for (int i = 0; i < datas.Count; i++)
        {
            if (i < miningRobotInventorySlots.Count)
            {
                miningRobotInventorySlots[i].Initialize(datas[i]);
            }
        }
    }
}
