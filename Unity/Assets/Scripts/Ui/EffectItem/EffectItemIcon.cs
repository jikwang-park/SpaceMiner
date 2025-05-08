using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectItemIcon : MonoBehaviour
{
    [SerializeField]
    private AddressableImage icon;
    [SerializeField]
    private TextMeshProUGUI levelText;
    
    public void Initialize(EffectItemTable.ItemType type, int level)
    {
        int itemId = DataTableManager.EffectItemTable.GetDatas(type)[level].NeedItemID;

        icon.SetItemSprite(itemId);
        levelText.text = $"Lv.{level}";
    }
}
