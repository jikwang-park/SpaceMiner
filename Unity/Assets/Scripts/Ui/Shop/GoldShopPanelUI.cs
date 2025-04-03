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
    private TextMeshProUGUI describeText;

    private Currency currentCurrency;
    private int currentSellPrice;
    private int currentGoldShopElementId;
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

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var datas = DataTableManager.ShopTable.GetList(ShopTable.ShopType.Gold);

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
                        goldShopElement.onClickGoldShopElement += SetGoldShopElement;
                    }
                }
                if(currentGoldShopElementId == 0)
                {
                    SetGoldShopElement(data.ID);
                }
            };
        }
    }
    public void SetGoldShopElement(int shopId)
    {
        currentGoldShopElementId = shopId;
        sellAmountSlider.value = 0f;
        currentCurrency = (Currency)DataTableManager.ShopTable.GetData(currentGoldShopElementId).NeedItemID;
        currentSellPrice = DataTableManager.ShopTable.GetData(currentGoldShopElementId).PayCount;
    }
    public void OnValueChangedSellAmount()
    {
        UpdateTexts();
    }
    public void UpdateTexts()
    {
        describeText.text = $"Sell Amount - {SellAmount}\nTotal Price - {TotalPrice}";
    }
    public void OnClickSellButton()
    {
        ItemManager.AddItem((int)currentCurrency, 1000);
        if (!ItemManager.CanConsume((int)currentCurrency, SellAmount))
        {
            Debug.Log($"OnClickSellButton : {currentCurrency}이 모자랍니다");
        }

        ItemManager.ConsumeCurrency(currentCurrency, SellAmount);
        ItemManager.AddItem((int)Currency.Gold, TotalPrice);
        sellAmountSlider.value = 0f;
    }
}
