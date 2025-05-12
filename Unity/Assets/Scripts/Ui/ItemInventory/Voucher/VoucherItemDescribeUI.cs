using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoucherItemDescribeUI : MonoBehaviour
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
}
