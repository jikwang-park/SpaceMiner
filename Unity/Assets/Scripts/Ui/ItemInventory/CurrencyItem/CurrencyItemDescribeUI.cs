using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyItemDescribeUI : MonoBehaviour
{
    [SerializeField]
    private AddressableImage icon;
    [SerializeField]
    private LocalizationText nameText;
    [SerializeField]
    private LocalizationText descriptionText;
    [SerializeField]
    private LocalizationText amountText;

    private int currentItemId;

    public void SetInfo(int itemId)
    {
        currentItemId = itemId;

        var currentItemData = DataTableManager.ItemTable.GetData(currentItemId);

        icon.SetItemSprite(currentItemId);
        nameText.SetString(currentItemData.NameStringID);
        descriptionText.SetString(currentItemData.DetailStringID);
        var amount = ItemManager.GetItemAmount(currentItemId);
        amountText.SetStringArguments(amount.ToString());
    }

    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemAmountChanged;
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemAmountChanged;
    }
    private void DoItemAmountChanged(int itemID, BigNumber count)
    {
        if (itemID != currentItemId)
        {
            return;
        }
        amountText.SetStringArguments(count.ToString());
    }
}
