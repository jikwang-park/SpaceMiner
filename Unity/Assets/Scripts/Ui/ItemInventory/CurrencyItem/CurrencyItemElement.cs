using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyItemElement : MonoBehaviour
{
    [SerializeField]
    private AddressableImage iconImage;

    [SerializeField]
    private TextMeshProUGUI amountText;

    public int itemId;
    public CurrencyItemInventory parentInventory;
    public void Initialize(int itemId)
    {
        this.itemId = itemId;

        var itemData = DataTableManager.ItemTable.GetData(itemId);
        iconImage.SetSprite(itemData.SpriteID);

        amountText.text = ItemManager.GetItemAmount(itemData.ID).ToString();
        ItemManager.OnItemAmountChanged += DoItemAmountChanged;
    }

    private void DoItemAmountChanged(int itemID, BigNumber count)
    {
        if (itemID != this.itemId)
        {
            return;
        }
        amountText.text = count.ToString();
    }

    public void OnClickCurrencyItem()
    {
        parentInventory.OnClickCurrencyItem(this);
    }
}
