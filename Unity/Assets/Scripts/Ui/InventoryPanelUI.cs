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
    private void Start()
    {
        InitializeInventories();
        DisplayInventory(UnitTypes.Tanker);
        displayTankerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Tanker));
        displayDealerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Dealer));
        displayHealerInvenButton.onClick.AddListener(() => DisplayInventory(UnitTypes.Healer));
    }

    public void DisplayInventory(UnitTypes type)
    {
        tankerInventory.gameObject.SetActive(false);
        dealerInventory.gameObject.SetActive(false);
        healerInventory.gameObject.SetActive(false);

        switch (type)
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
            tankerInventory.Initialize(typeDict[UnitTypes.Tanker]);
        }
        if (typeDict.ContainsKey(UnitTypes.Dealer))
        {
            dealerInventory.Initialize(typeDict[UnitTypes.Dealer]);
        }
        if (typeDict.ContainsKey(UnitTypes.Healer))
        {
            healerInventory.Initialize(typeDict[UnitTypes.Healer]);
        }
    }
}
