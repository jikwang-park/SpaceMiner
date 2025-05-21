using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DungeonClearRewardIcon : MonoBehaviour, IObjectPoolGameObject
{
    [SerializeField]
    private AddressableImage itemIcon;
    [SerializeField]
    private TextMeshProUGUI itemAmountText;

    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }

    public void SetItem(int itemId)
    {
        itemIcon.SetItemSprite(itemId);

        itemAmountText.gameObject.SetActive(false);
    }

    public void SetItem(int itemId, BigNumber amount)
    {
        itemIcon.SetItemSprite(itemId);

        itemAmountText.gameObject.SetActive(true);
        itemAmountText.text = amount.ToString();
    }

    public void SetItem(int itemId, BigNumber minAmount, BigNumber maxAmount)
    {
        itemIcon.SetItemSprite(itemId);

        itemAmountText.gameObject.SetActive(true);
        itemAmountText.text = $"{minAmount}~{maxAmount}";
    }
}
