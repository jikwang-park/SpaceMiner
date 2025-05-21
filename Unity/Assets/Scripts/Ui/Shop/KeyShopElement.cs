using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyShopElement : MonoBehaviour
{
    [SerializeField]
    private AddressableImage keyIcon;
    [SerializeField]
    private AddressableImage currencyIcon;
    [SerializeField]
    private LocalizationText needItemCountText;
    [SerializeField]
    private LocalizationText dailyPurchaseText;
    [SerializeField]
    private Button purchaseButton;

    private int currentElementId;
    private int needItemId;
    private int paymentItemId;

    private int dailyResetHour;
    private int dailyResetMinute;

    private BigNumber paymentItemAmount;
    private BigNumber needItemAmount;
    private int dailyPurchaseLimitCount;
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
        int needItemStringId = DataTableManager.ItemTable.GetData(needItemId).NameStringID;
        needItemString = DataTableManager.StringTable.GetData(needItemStringId);

        int paymentItemSpriteId = DataTableManager.ItemTable.GetData(paymentItemId).SpriteID;
        keyIcon.SetSprite(paymentItemSpriteId);

        int needItemSpriteId = DataTableManager.ItemTable.GetData(needItemId).SpriteID;
        currencyIcon.SetSprite(needItemSpriteId);

        CheckReset();
    }
    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemChange;
        if(currentData != null)
        {
            CheckReset();
        }
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemChange;
    }
    private void DoItemChange(int itemId, BigNumber amount)
    {
        if(itemId == needItemId)
        {
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        needItemCountText.SetStringArguments(needItemString, needItemAmount.ToString());
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
