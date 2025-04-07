using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningRobotShopElement : MonoBehaviour
{
    [SerializeField]
    private Image robotIcon;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private TextMeshProUGUI decribeText;
    [SerializeField]
    private Button button;

    private int needItemId;
    private int paymentItemId;

    private BigNumber needAmount;
    private int paymentItemAmount;

    private string describeTextFormat = "Need Amount To buy : {0}";

    public void Initialize(ShopTable.Data data)
    {
        needItemId = data.NeedItemID;
        paymentItemId = data.PaymentItemID;

        needAmount = data.NeedCount;
        paymentItemAmount = data.PayCount;
        UpdateUI();
    }
    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemAmountChanged;
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemAmountChanged;

    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ItemManager.AddItem(needItemId, 100);
        }
    }
#endif
    private void UpdateUI()
    {
        decribeText.text = string.Format(describeTextFormat, needAmount);
        button.interactable = ItemManager.CanConsume(needItemId, needAmount);
    }
    public void OnClickBuyButton()
    {
        if(ItemManager.CanConsume(needItemId, needAmount))
        {
            ItemManager.ConsumeItem(needItemId, needAmount);
            ItemManager.AddItem(paymentItemId, paymentItemAmount);
            UpdateUI();
        }
    }
    private void DoItemAmountChanged(int itemId, BigNumber amount)
    {
        if(needItemId == itemId)
        {
            UpdateUI();
        }
    }
}
