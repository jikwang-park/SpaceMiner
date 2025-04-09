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
    public void UpdateUI()
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
