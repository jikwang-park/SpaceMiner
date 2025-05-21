using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField] 
    private Sprite deselectedSprite;
    [SerializeField]
    private Toggle tankerToggle;
    [SerializeField]
    private Toggle dealerToggle;
    [SerializeField]
    private Toggle healerToggle;

    [SerializeField]
    private Inventory tankerInventory;
    [SerializeField]
    private Inventory dealerInventory;
    [SerializeField]
    private Inventory healerInventory;

    private Image tankerToggleImage;
    private Image dealerToggleImage;
    private Image healerToggleImage;

    private Dictionary<UnitTypes, Inventory> inventories = new Dictionary<UnitTypes, Inventory>();
    private UnitTypes currentType = UnitTypes.Healer;
    public void Awake()
    {
        InitializeInventories();
        InventoryManager.onChangedInventory += ApplyChangesInventorys;
        inventories.Add(UnitTypes.Tanker, tankerInventory);
        inventories.Add(UnitTypes.Dealer, dealerInventory);
        inventories.Add(UnitTypes.Healer, healerInventory);
        tankerToggleImage = tankerToggle.GetComponent<Image>();
        dealerToggleImage = dealerToggle.GetComponent<Image>();
        healerToggleImage = healerToggle.GetComponent<Image>();
    }
    public void OnEnable()
    {
        tankerToggle.isOn = false;
        dealerToggle.isOn = false;
        healerToggle.isOn = false;
        tankerToggle.isOn = true;
    }
    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
    public void OnProcessToggles()
    {
        if (tankerToggle.isOn)
        {
            OnClickDisplayTankerInventoryButton();
        }
        else if (dealerToggle.isOn)
        {
            OnClickDisplayDealerInventoryButton();
        }
        else if (healerToggle.isOn)
        {
            OnClickDisplayHealerInventoryButton();
        }

        UpdateToggleSprites();
    }
    private void UpdateToggleSprites()
    {
        tankerToggleImage.sprite = tankerToggle.isOn ? selectedSprite : deselectedSprite;
        healerToggleImage.sprite = healerToggle.isOn ? selectedSprite : deselectedSprite;
        dealerToggleImage.sprite = dealerToggle.isOn ? selectedSprite : deselectedSprite;
    }
    public void DisplayInventory(UnitTypes type)
    {
        if(currentType == type)
        {
            return;
        }

        inventories[currentType].gameObject.SetActive(false);
        currentType = type;
        inventories[currentType].gameObject.SetActive(true);
    }

    private void InitializeInventories()
    {
        tankerInventory.Initialize(UnitTypes.Tanker);
        dealerInventory.Initialize(UnitTypes.Dealer);
        healerInventory.Initialize(UnitTypes.Healer);
    }
    public void OnClickBatchMergeButton()
    {
        switch (currentType)
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
        if(currentType == UnitTypes.Tanker)
        {
            return;
        }
        DisplayInventory(UnitTypes.Tanker);
    }
    public void OnClickDisplayDealerInventoryButton()
    {
        if (currentType == UnitTypes.Dealer)
        {
            return;
        }
        DisplayInventory(UnitTypes.Dealer);
    }
    public void OnClickDisplayHealerInventoryButton()
    {
        if (currentType == UnitTypes.Healer)
        {
            return;
        }
        DisplayInventory(UnitTypes.Healer);
    }
}
