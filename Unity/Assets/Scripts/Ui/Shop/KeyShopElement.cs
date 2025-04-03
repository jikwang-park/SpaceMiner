using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyShopElement : MonoBehaviour
{
    [SerializeField]
    private Image keyIcon;
    [SerializeField]
    private Image currencyIcon;
    [SerializeField]
    private TextMeshProUGUI paymentItemCountText;
    [SerializeField]
    private TextMeshProUGUI needItemCountText;
    [SerializeField]
    private TextMeshProUGUI dailyPurchaseText;
    [SerializeField]
    private Button purchaseButton;

    private int currentElementId;
    private int needItemId;
    private int paymentItemId;

    private string needItemCountFormat = "Need {0} item : {1}";
    private string dailyPurchaseFormat = "Buy\n({0}/{1})";

    private int paymentItemAmount;
    private int needItemAmount;
    private int dailyPurchaseCount = 0;
    private int dailyPurchaseLimitCount;
    private bool CanPurchase
    {
        get
        {
            return (dailyPurchaseCount >= dailyPurchaseLimitCount && ItemManager.CanConsume(needItemId, needItemAmount));
        }
    }

    public void Initialize(ShopTable.Data data)
    {
        currentElementId = data.ID;
        needItemId = data.NeedItemID;
        paymentItemId = data.PaymentItemID;

        paymentItemAmount = data.PayCount;
        needItemAmount = data.NeedCount;
        dailyPurchaseLimitCount = data.DailyPurchaseLimit;

        UpdateUI();
    }
    private void UpdateUI()
    {
        paymentItemCountText.text = $"{paymentItemId} * {paymentItemAmount}";
        needItemCountText.text = string.Format(needItemCountFormat, needItemId, needItemAmount);
        dailyPurchaseText.text = string.Format(dailyPurchaseFormat, dailyPurchaseCount, dailyPurchaseLimitCount);

        purchaseButton.interactable = CanPurchase;
    }
    public void OnClickPurchaseButton()
    {
        if(CanPurchase)
        {
            ItemManager.ConsumeItem(needItemId, needItemAmount);
            ItemManager.AddItem(paymentItemId, paymentItemAmount);
            dailyPurchaseCount++;
            UpdateUI();
        }
    }
}
