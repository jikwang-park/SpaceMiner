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
    private void Start()
    {
        InitializeInventories();
        DisplayInventory(UnitTypes.Tanker);
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
    private void DoSave(TotalSaveData totalSaveData)
    {
        totalSaveData.inventorySaveData[UnitTypes.Tanker] = tankerInventory.Save();
        totalSaveData.inventorySaveData[UnitTypes.Dealer] = dealerInventory.Save();
        totalSaveData.inventorySaveData[UnitTypes.Healer] = healerInventory.Save();
    }
}
