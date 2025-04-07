using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaPurchaseButton : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private TextMeshProUGUI countText;

    private BigNumber cost;
    private int count;

    public event Action<int> onClickGachaButton;

    public void Initialize(int count, BigNumber cost, Sprite sprite = null)
    {
        this.count = count;
        this.cost = cost;

        // iconImage.sprite = sprite;
        countText.text = $"{this.count} Times";
        costText.text = $"{this.cost}";
    }

    public void OnClickGachaButton()
    {
        onClickGachaButton?.Invoke(count);
    }
}
