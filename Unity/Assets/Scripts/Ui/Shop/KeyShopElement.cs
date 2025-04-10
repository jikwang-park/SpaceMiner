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

    private int dailyResetHour;
    private int dailyResetMinute;

    private string needItemCountFormat = "Need {0} item : {1}";
    private string dailyPurchaseFormat = "Buy\n({0}/{1})";

    private BigNumber paymentItemAmount;
    private BigNumber needItemAmount;
    private int dailyPurchaseLimitCount;

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
        needItemAmount = shopData.NeedCount;
        dailyPurchaseLimitCount = shopData.DailyPurchaseLimit;
        dailyResetHour = (shopData.ResetTime / 100) % 24;
        dailyResetMinute = shopData.ResetTime % 100;
        StartCoroutine(CheckReset());
        UpdateUI();
    }
    private void OnEnable()
    {
        StartCoroutine(CheckReset());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ItemManager.AddItem(1002, 5000);
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        paymentItemCountText.text = $"{paymentItemId} * {paymentItemAmount}";
        needItemCountText.text = string.Format(needItemCountFormat, needItemId, needItemAmount);
        dailyPurchaseText.text = string.Format(dailyPurchaseFormat, currentData.dailyPurchaseCount, dailyPurchaseLimitCount);

        purchaseButton.interactable = CanPurchase;
    }
    public void OnClickPurchaseButton()
    {
        if(CanPurchase)
        {
            ItemManager.ConsumeItem(needItemId, needItemAmount);
            ItemManager.AddItem(paymentItemId, paymentItemAmount);
            currentData.dailyPurchaseCount++;
            StartCoroutine(UpdateLastPurchaseTime());
            UpdateUI();
        }
    }
    private IEnumerator CheckReset()
    {
        yield return StartCoroutine(TimeManager.Instance.GetServerTime((DateTime serverTime) =>
        {
            if (serverTime == DateTime.MinValue)
            {
                return;
            }
            DateTime currentDate = serverTime.Date;
            DateTime lastPurchaseDate = currentData.lastPurchaseTime.Date;

            if (currentData.lastPurchaseTime == DateTime.MinValue || (currentData.lastPurchaseTime.Date < serverTime.Date && serverTime.TimeOfDay >= new TimeSpan(dailyResetHour, dailyResetMinute, 0)))
            {
                currentData.dailyPurchaseCount = 0;
            }
            else
            {
                Debug.Log("오늘 구매 기록이 있음: 마지막 구매 날짜 = " + lastPurchaseDate.ToShortDateString());
            }

            UpdateUI();
        }));
    }
    private IEnumerator UpdateLastPurchaseTime()
    {
        yield return StartCoroutine(TimeManager.Instance.GetServerTime((DateTime serverTime) =>
        {
            if (serverTime != DateTime.MinValue)
            {
                currentData.lastPurchaseTime = serverTime;
            }
        }));
    }
}
