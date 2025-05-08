using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectItemDescribeUI : MonoBehaviour
{
    [SerializeField]
    private EffectItemIcon icon;
    [SerializeField]
    private LocalizationText itemNameText;
    [SerializeField]
    private LocalizationText currentLevelText;
    [SerializeField]
    private LocalizationText nextLevelText;

    public void SetInfo(EffectItemTable.ItemType type, int level)
    {
        icon.Initialize(type, level);
    }
}
