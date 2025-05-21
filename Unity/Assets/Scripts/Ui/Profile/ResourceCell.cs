using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceCell : MonoBehaviour
{
    [SerializeField]
    private int targetItemID;

    [SerializeField]
    private AddressableImage itemIcon;

    [SerializeField]
    private TextMeshProUGUI amountText;

    private void Start()
    {
        if (targetItemID == 0)
        {
            return;
        }

        itemIcon.SetItemSprite(targetItemID);
        amountText.text = ItemManager.GetItemAmount(targetItemID).ToString();
        ItemManager.OnItemAmountChanged += OnItemAmountChanged;
    }

    private void OnItemAmountChanged(int itemId, BigNumber amount)
    {
        if (itemId != targetItemID)
        {
            return;
        }
        amountText.text = amount.ToString();
    }
}
