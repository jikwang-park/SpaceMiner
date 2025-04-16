using System;
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
    private TextMeshProUGUI needItemCountText;
    [SerializeField]
    private LocalizationText dailyPurchaseText;
    [SerializeField]
    private Button purchaseButton;

    private int currentElementId;
    private int needItemId;
    private int paymentItemId;

    private int dailyResetHour;
    private int dailyResetMinute;

    private string needItemCountFormat = "필요한 {0} : {1}";

    private BigNumber paymentItemAmount;
    private BigNumber needItemAmount;
    private int dailyPurchaseLimitCount;

    private string paymentItemString;
    private string needItemString;

    private DungeonKeyShopElementData currentData;
    private bool CanPurchase
    {
        get
        {
            return (currentData.dailyPurchaseCount < dailyPurchaseLimitCount && ItemManager.CanConsume(needItemId, needItemAmount));
        }
    }
    public void Initialize(DungeonKeyShopElementData data)
    {
        currentData = data;
        currentElementId = data.shopId;

        var shopData = DataTableManager.ShopTable.GetData(currentElementId);

        needItemId = shopData.NeedItemID;
        paymentItemId = shopData.PaymentItemID;
        paymentItemAmount = shopData.PayCount;
        needItemAmount = shopData.NeedItemCount;
        dailyPurchaseLimitCount = shopData.DailyPurchaseLimit;

        dailyResetHour = (shopData.ResetTime / 100) % 24;
        dailyResetMinute = shopData.ResetTime % 100;

        int paymentItemStringId = DataTableManager.ItemTable.GetData(paymentItemId).NameStringID;
        paymentItemString = DataTableManager.StringTable.GetData(paymentItemStringId);

        int needItemStringId = DataTableManager.ItemTable.GetData(needItemId).NameStringID;
        needItemString = DataTableManager.StringTable.GetData(needItemStringId);

        CheckReset();
    }
    private void OnEnable()
    {
        if(currentData != null)
        {
            CheckReset();
        }
    }
    private void UpdateUI()
    {
        needItemCountText.text = string.Format(needItemCountFormat, needItemString, needItemAmount);
        dailyPurchaseText.SetStringArguments(currentData.dailyPurchaseCount.ToString(), dailyPurchaseLimitCount.ToString());

        purchaseButton.interactable = CanPurchase;
    }
    public void OnClickPurchaseButton()
    {
        if(CanPurchase)
        {
            ItemManager.ConsumeItem(needItemId, needItemAmount);
            ItemManager.AddItem(paymentItemId, paymentItemAmount);
            currentData.dailyPurchaseCount++;
            UpdateLastPurchaseTime();
            UpdateUI();
        }
    }
    private void CheckReset()
    {
        DateTime estimatedTime = TimeManager.Instance.GetEstimatedServerTime();

        if (estimatedTime == DateTime.MinValue)
        {
            Debug.LogWarning("추정 서버 시간이 유효하지 않습니다.");
            return;
        }

        TimeSpan resetThreshold = new TimeSpan(dailyResetHour, dailyResetMinute, 0);

        if (currentData.lastPurchaseTime == DateTime.MinValue ||
            (currentData.lastPurchaseTime.Date < estimatedTime.Date && estimatedTime.TimeOfDay >= resetThreshold))
        {
            currentData.dailyPurchaseCount = 0;
        }

        UpdateUI();
    }
    private void UpdateLastPurchaseTime()
    {
        DateTime estimatedTime = TimeManager.Instance.GetEstimatedServerTime();
        if (estimatedTime != DateTime.MinValue)
        {
            currentData.lastPurchaseTime = estimatedTime;
            SaveLoadManager.SaveGame();
        }
    }
}
