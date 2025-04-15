using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInventoryChild : MonoBehaviour
{
    [SerializeField]
    private int itemID;

    [SerializeField]
    private AddressableImage iconImage;

    [SerializeField]
    private TextMeshProUGUI countText;

    private void Start()
    {
        var itemData = DataTableManager.ItemTable.GetData(itemID);
        iconImage.SetSprite(itemData.SpriteID);

        countText.text = ItemManager.GetItemAmount(itemData.ID).ToString();
        ItemManager.OnItemAmountChanged += ItemAmountChanged;
    }

    private void ItemAmountChanged(int itemID, BigNumber count)
    {
        if (itemID != this.itemID)
        {
            return;
        }
        countText.text = count.ToString();
    }
}
