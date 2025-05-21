using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GoldShopPanelUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private Slider sellAmountSlider;
    [SerializeField]
    private LocalizationText sellAmountText;
    [SerializeField]
    private LocalizationText totalPriceText;
    [SerializeField]
    private AddressableImage icon;

    private ToggleGroup toggleGroup;
    private Currency currentCurrency;
    private int currentSellPrice;
    private int currentGoldShopElementId;
    private int defaultElementId;

    private List<GoldShopElement> elements = new List<GoldShopElement>();
    public BigNumber SellAmount
    {
        get
        {
            return ItemManager.GetItemAmount((int)currentCurrency) * sellAmountSlider.value;
        }
    }
    public BigNumber TotalPrice
    {
        get
        {
            return SellAmount * currentSellPrice;
        }
    }
    private string prefabFormat = "Prefabs/UI/GoldShopElement";
    private void OnEnable()
    {
        SetGoldShopElement(defaultElementId);
        UpdateUI();
        ItemManager.OnItemAmountChanged += DoItemChanged;
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemChanged;
    }
    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        toggleGroup = contentParent.GetComponent<ToggleGroup>();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        int instantiatedCount = 0;
        var datas = DataTableManager.ShopTable.GetList(ShopTable.ShopType.Gold);
        int totalCount = datas.Count;
        defaultElementId = datas[0].ID;
        foreach (var data in datas)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    GoldShopElement goldShopElement = elementObj.GetComponent<GoldShopElement>();
                    if (goldShopElement != null)
                    {
                        goldShopElement.Initialize(data);
                        goldShopElement.toggle.group = toggleGroup;
                        goldShopElement.onClickGoldShopElement += SetGoldShopElement;
                        elements.Add(goldShopElement);
                    }
                }
                instantiatedCount++;

                if (instantiatedCount == totalCount && elements.Count > 0)
                {
                    elements[0].toggle.isOn = true;
                }
            };
        }
    }
    public void SetGoldShopElement(int shopId)
    {
        currentGoldShopElementId = shopId;
        sellAmountSlider.value = 0f;
        currentCurrency = (Currency)DataTableManager.ShopTable.GetData(currentGoldShopElementId).NeedItemID;
        icon.SetItemSprite((int)currentCurrency);
        currentSellPrice = DataTableManager.ShopTable.GetData(currentGoldShopElementId).PayCount;
        UpdateUI();
    }
    public void OnValueChangedSellAmount()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        sellAmountText.SetStringArguments(SellAmount.ToString());
        totalPriceText.SetStringArguments(TotalPrice.ToString());
        sellAmountSlider.interactable = ItemManager.GetItemAmount((int)currentCurrency) > 0;
    }
    public void OnClickSellButton()
    {
        if (!ItemManager.CanConsume((int)currentCurrency, SellAmount))
        {
            Debug.Log($"OnClickSellButton : {currentCurrency}이 모자랍니다");
        }
        var totalPrice = TotalPrice;
        ItemManager.ConsumeCurrency(currentCurrency, SellAmount);
        ItemManager.AddItem((int)Currency.Gold, totalPrice);
        sellAmountSlider.value = 0f;
    }
    private void DoItemChanged(int itemId, BigNumber amount)
    {
        if((int)currentCurrency == itemId)
        {
            UpdateUI();
        }
    }
}
