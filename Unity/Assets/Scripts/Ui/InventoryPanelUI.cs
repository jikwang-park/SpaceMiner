using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField]
    private Inventory tankerInventory;
    [SerializeField]
    private Inventory dealerInventory;
    [SerializeField]
    private Inventory healerInventory;
    [SerializeField]
    private Button displayTankerInvenButton;
    [SerializeField]
    private Button displayDealerInvenButton;
    [SerializeField]
    private Button displayHealerInvenButton;
    [SerializeField]
    private Button BatchMergeButton;

    private UnitTypes currentInventoryType;
    private int inventoriesInitializedCount = 0;
    private const int totalInventories = 3;
    private void Start()
    {
        InitializeInventories();
        SaveLoadManager.LoadGame();
        displayTankerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Tanker));
        displayDealerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Dealer));
        displayHealerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Healer));
        BatchMergeButton.onClick.AddListener(() => OnClickBatchMergeButton());
    }

    public void DisplayInventory(UnitTypes type)
    {
        if(currentInventoryType == type)
        {
            return;
        }

        currentInventoryType = type;
        tankerInventory.gameObject.SetActive(false);
        dealerInventory.gameObject.SetActive(false);
        healerInventory.gameObject.SetActive(false);

        switch (currentInventoryType)
        {
            case UnitTypes.Tanker:
                tankerInventory.gameObject.SetActive(true);
                break;
            case UnitTypes.Dealer:
                dealerInventory.gameObject.SetActive(true);
                break;
            case UnitTypes.Healer:
                healerInventory.gameObject.SetActive(true);
                break;
        }
    }

    private void InitializeInventories()
    {
        Dictionary<UnitTypes, List<SoldierTable.Data>> typeDict = DataTableManager.SoldierTable.GetTypeDictionary();

        if (typeDict.ContainsKey(UnitTypes.Tanker))
        {
            tankerInventory.Initialize(typeDict[UnitTypes.Tanker], UnitTypes.Tanker);
        }
        if (typeDict.ContainsKey(UnitTypes.Dealer))
        {
            dealerInventory.Initialize(typeDict[UnitTypes.Dealer], UnitTypes.Dealer);
        }
        if (typeDict.ContainsKey(UnitTypes.Healer))
        {
            healerInventory.Initialize(typeDict[UnitTypes.Healer], UnitTypes.Healer);
        }
    }
    private void InventoryInitialized()
    {
        inventoriesInitializedCount++;
        if (inventoriesInitializedCount >= totalInventories)
        {
            DoLoad(SaveLoadManager.LoadedData);
            DisplayInventory(UnitTypes.Tanker);
        }
    }
    private void OnClickBatchMergeButton()
    {
        switch (currentInventoryType)
        {
            case UnitTypes.Tanker:
                tankerInventory.BatchMerge();
                break;
            case UnitTypes.Dealer:
                dealerInventory.BatchMerge();
                break;
            case UnitTypes.Healer:
                healerInventory.BatchMerge();
                break;
        }
    }

    private void OnEnable()
    {
        tankerInventory.OnInitialized += InventoryInitialized;
        dealerInventory.OnInitialized += InventoryInitialized;
        healerInventory.OnInitialized += InventoryInitialized;
        SaveLoadManager.onSaveRequested += DoSave;
    }

    private void OnDisable()
    {
        tankerInventory.OnInitialized -= InventoryInitialized;
        dealerInventory.OnInitialized -= InventoryInitialized;
        healerInventory.OnInitialized -= InventoryInitialized;
        SaveLoadManager.onSaveRequested -= DoSave;
    }
    private void DoSave(TotalSaveData totalSaveData)
    {
        totalSaveData.inventorySaveData[UnitTypes.Tanker] = tankerInventory.Save();
        totalSaveData.inventorySaveData[UnitTypes.Dealer] = dealerInventory.Save();
        totalSaveData.inventorySaveData[UnitTypes.Healer] = healerInventory.Save();
    }
    private void DoLoad(TotalSaveData totalSaveData)
    {
        if (SaveLoadManager.LoadedData == null)
        {
            Debug.Log("저장된 데이터가 없습니다. 기본 값으로 진행합니다.");
            return;
        }

        if (totalSaveData.inventorySaveData.TryGetValue(UnitTypes.Tanker, out InventorySaveData tankerData))
        {
            tankerInventory.Load(tankerData);
        }
        if (totalSaveData.inventorySaveData.TryGetValue(UnitTypes.Dealer, out InventorySaveData dealerData))
        {
            dealerInventory.Load(dealerData);
        }
        if (totalSaveData.inventorySaveData.TryGetValue(UnitTypes.Healer, out InventorySaveData healerData))
        {
            healerInventory.Load(healerData);
        }
    }
}
