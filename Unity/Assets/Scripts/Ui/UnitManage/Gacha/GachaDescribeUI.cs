using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaDescribeUI : MonoBehaviour
{
    [SerializeField]
    private AddressableImage currencyItemIcon;
    [SerializeField]
    private AddressableImage voucherItemIcon;
    [SerializeField]
    private LocalizationText currencyAmountText;
    [SerializeField] 
    private LocalizationText voucherAmountText;
    [SerializeField]
    private LocalizationText describeText;
    private Image image;

    private int currencyItemId;
    private int voucherItemId;
    private void Awake()
    {
        image = GetComponent<Image>();
        ItemManager.OnItemAmountChanged += DoItemAmountChanged;
    }

    private void DoItemAmountChanged(int itemId, BigNumber number)
    {
        if(itemId == currencyItemId) 
        {
            currencyAmountText.SetStringArguments(ItemManager.GetItemAmount(currencyItemId).ToString());
        }
        else if(itemId == voucherItemId)
        {
            voucherAmountText.SetStringArguments(ItemManager.GetItemAmount(voucherItemId).ToString());
        }
    }

    public void Initialize(GachaTable.Data data, Sprite backgroundSprite)
    {
        currencyItemId = data.NeedItemID1;
        voucherItemId = data.NeedItemID2;

        describeText.SetString(data.DetailStringID);
        image.sprite = backgroundSprite;

        currencyItemIcon.SetItemSprite(currencyItemId);
        voucherItemIcon.SetItemSprite(voucherItemId);

        currencyAmountText.SetStringArguments(ItemManager.GetItemAmount(currencyItemId).ToString());
        voucherAmountText.SetStringArguments(ItemManager.GetItemAmount(voucherItemId).ToString());
    }

}
