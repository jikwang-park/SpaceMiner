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

    private Image iconImage;
    private Image backgroundImage;
    private bool isLocked = true;
    private static readonly Color LockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    private static readonly Color UnlockedColor = Color.white;
    private void Awake()
    {
        iconImage = icon.GetComponent<Image>();
        backgroundImage = GetComponent<Image>();
    }
    public void Initialize(EffectItemTable.ItemType type, int level)
    {
        int itemId = DataTableManager.EffectItemTable.GetDatas(type)[level].NeedItemID;

        icon.SetItemSprite(itemId);
        levelText.text = $"Lv.{level}";

        if((level == 0) != isLocked)
        {
            isLocked = (level == 0);
            ApplyColor(isLocked);
        }
    }
    private void ApplyColor(bool locked)
    {
        var color = locked ? LockedColor : UnlockedColor;
        iconImage.color = color;
        backgroundImage.color = color;
        levelText.color = color;
    }
}
