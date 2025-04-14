using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleRewardElement : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TextMeshProUGUI amountText;

    public void Initialize(int itemId, BigNumber amount)
    {
        // icon.sprite = ;

        amountText.text = $"{(Currency)itemId} : {amount.ToString()}";
    }
}
