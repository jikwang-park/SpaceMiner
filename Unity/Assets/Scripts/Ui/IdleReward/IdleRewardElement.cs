using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardElement : MonoBehaviour
{
    [SerializeField]
    private AddressableImage icon;
    [SerializeField]
    private TextMeshProUGUI amountText;

    public void Initialize(int itemId, BigNumber amount)
    {
        var itemData = DataTableManager.ItemTable.GetData(itemId);
        icon.SetSprite(itemData.SpriteID);
        string itemString = DataTableManager.StringTable.GetData(itemData.NameStringID);
        amountText.text = $"{itemString} : {amount.ToString()} È¹µæ";
    }
}
