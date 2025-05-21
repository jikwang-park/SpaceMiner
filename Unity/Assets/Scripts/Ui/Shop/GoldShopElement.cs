using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldShopElement : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI mineralNameText;
    [SerializeField]
    private TextMeshProUGUI NeedAmountText;
    [SerializeField]
    private TextMeshProUGUI SellRatioText;

    private int shopId;
    private Currency currencyType;
    private BigNumber payAmount;
    public event Action<int> onClickGoldShopElement;

    private string sellRatioFormat = "1 : {0}";

    public void Initialize(ShopTable.Data data)
    {
        shopId = data.ID;
        currencyType = (Currency)data.NeedItemID;
        payAmount = data.PayCount;

        UpdateUI();
    }
    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemChange;
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemChange;
    }
    private void UpdateUI()
    {
        mineralNameText.text = currencyType.ToString();
        NeedAmountText.text = $"{ItemManager.GetItemAmount((int)currencyType)}";
        SellRatioText.text = string.Format(sellRatioFormat, payAmount);
    }

    public void OnClickGoldShopElement()
    {
        onClickGoldShopElement?.Invoke(shopId);
    }
    private void DoItemChange(int itemId, BigNumber amount)
    {
        if((int)currencyType == itemId)
        {
            NeedAmountText.text = $"{amount}";
        }
    }
}
