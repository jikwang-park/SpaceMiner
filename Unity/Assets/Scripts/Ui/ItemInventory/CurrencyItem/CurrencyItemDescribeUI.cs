using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyItemDescribeUI : MonoBehaviour
{
    [SerializeField]
    private LocalizationText nameText;
    [SerializeField]
    private LocalizationText descriptionText;
    [SerializeField]
    private TextMeshProUGUI amountText;

    private int currentItemId;

    public void SetInfo(int itemId)
    {
        currentItemId = itemId;

        var currentItemData = DataTableManager.ItemTable.GetData(currentItemId);

        nameText.SetString(currentItemData.NameStringID);
        // descriptionText.SetString(currentItemData.)
        var amount = ItemManager.GetItemAmount(currentItemId);
        amountText.text = amount.ToString();
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
        amountText.text = count.ToString();
    }
}
