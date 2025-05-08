using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Image arrowImage;

    public void SetInfo(EffectItemTable.ItemType type, int level)
    {
        icon.Initialize(type, level);


        var currentLevelData = DataTableManager.EffectItemTable.GetDatas(type)[level];
        int itemId = currentLevelData.NeedItemID;
        int itemStringId = DataTableManager.ItemTable.GetData(itemId).NameStringID;

        itemNameText.SetString(itemStringId);
        //currentLevelText.SetString(currentLevelData.);

        bool isMaxLevel = level >= DataTableManager.EffectItemTable.GetDatas(type).Count - 1;
        arrowImage.gameObject.SetActive(isMaxLevel);

    }
}
