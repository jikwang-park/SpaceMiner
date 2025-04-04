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

    private UnitTypes currentInventoryType;
    public void Awake()
    {
        InitializeInventories();
        InventoryManager.onChangedInventory += ApplyChangesInventorys;
    }
    public void OnEnable()
    {
        DisplayInventory(UnitTypes.Tanker);
    }
    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
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
        tankerInventory.Initialize(UnitTypes.Tanker);
        dealerInventory.Initialize(UnitTypes.Dealer);
        healerInventory.Initialize(UnitTypes.Healer);
    }
    public void OnClickBatchMergeButton()
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
    public void ApplyChangesInventorys()
    {
        tankerInventory.ApplyChangesInventory();
        dealerInventory.ApplyChangesInventory();
        healerInventory.ApplyChangesInventory();
    }
    public void OnClickDisplayTankerInventoryButton()
    {
        if(currentInventoryType == UnitTypes.Tanker)
        {
            return;
        }
        DisplayInventory(UnitTypes.Tanker);
    }
    public void OnClickDisplayDealerInventoryButton()
    {
        if (currentInventoryType == UnitTypes.Dealer)
        {
            return;
        }
        DisplayInventory(UnitTypes.Dealer);
    }
    public void OnClickDisplayHealerInventoryButton()
    {
        if (currentInventoryType == UnitTypes.Healer)
        {
            return;
        }
        DisplayInventory(UnitTypes.Healer);
    }
}
