using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldShopElement : MonoBehaviour
{
    [SerializeField]
    private AddressableImage icon;
    [SerializeField]
    private LocalizationText mineralNameText;
    [SerializeField]
    private TextMeshProUGUI NeedAmountText;
    [SerializeField]
    private TextMeshProUGUI SellRatioText;

    private int shopId;
    private Currency currencyType;
    private BigNumber payAmount;
    public event Action<int> onClickGoldShopElement;

    private bool isInitialized = false;
    public void Initialize(ShopTable.Data data)
    {
        int itemSpriteId = DataTableManager.ItemTable.GetData(data.NeedItemID).SpriteID;

        icon.SetSprite(itemSpriteId);
        shopId = data.ID;
        currencyType = (Currency)data.NeedItemID;
        payAmount = data.PayCount;
        isInitialized = true;

        UpdateUI();
    }
    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemChange;
        if(isInitialized)
        {
            UpdateUI();
        }
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemChange;
    }
    private void UpdateUI()
    {
        var data = DataTableManager.ItemTable.GetData((int)currencyType);
        mineralNameText.SetString(data.NameStringID);
        NeedAmountText.text = $"{ItemManager.GetItemAmount((int)currencyType)}";
        SellRatioText.text = payAmount.ToString();
    }

    public void OnClickGoldShopElement()
    {
        onClickGoldShopElement?.Invoke(shopId);
    }
    private void DoItemChange(int itemId, BigNumber amount)
    {
        if((int)currencyType == itemId)
        {
            UpdateUI();
        }
    }
}
