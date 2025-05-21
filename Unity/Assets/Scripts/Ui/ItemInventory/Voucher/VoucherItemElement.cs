using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VoucherItemElement : MonoBehaviour
{
    [SerializeField]
    private AddressableImage iconImage;

    [SerializeField]
    private TextMeshProUGUI amountText;

    public int itemId;
    public VoucherItemInventory parentInventory;
    public void Initialize(int itemId)
    {
        this.itemId = itemId;

        var itemData = DataTableManager.ItemTable.GetData(itemId);
        iconImage.SetSprite(itemData.SpriteID);

        amountText.text = ItemManager.GetItemAmount(itemData.ID).ToString();
    }

    private void OnEnable()
    {
        if(itemId != 0)
        {
            amountText.text = ItemManager.GetItemAmount(itemId).ToString();
        }
    }

    public void OnClickCurrencyItem()
    {
        parentInventory.OnClickVoucherItem(this);
    }
}
