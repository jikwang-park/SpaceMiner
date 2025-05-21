using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaPurchaseButton : MonoBehaviour
{
    [SerializeField]
    private AddressableImage image;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private LocalizationText countText;

    private BigNumber cost;
    private int count;
    public event Action<int> onClickGachaButton;
    private void Awake()
    {
    }
    public void Initialize(int count, BigNumber cost, int itemId)
    {
        this.count = count;
        this.cost = cost;

        countText.SetStringArguments(count.ToString());
        costText.text = $"{this.cost}";
        image.SetSprite(DataTableManager.ItemTable.GetData(itemId).SpriteID);
    }

    public void OnClickGachaButton()
    {
        onClickGachaButton?.Invoke(count);
    }
}
