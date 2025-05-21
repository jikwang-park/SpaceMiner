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
        float currentLevelValue = currentLevelData.Value < 1f ? currentLevelData.Value * 100 : currentLevelData.Value;
        currentLevelText.SetString(currentLevelData.DetailStringID, currentLevelValue.ToString());

        if(level >= DataTableManager.EffectItemTable.GetDatas(type).Count - 1)
        {
            arrowImage.gameObject.SetActive(false);
            nextLevelText.gameObject.SetActive(false);
        }
        else
        {
            arrowImage.gameObject.SetActive(true);
            nextLevelText.gameObject.SetActive(true);
            var nextLevelData = DataTableManager.EffectItemTable.GetDatas(type)[level + 1];
            float nextLevelValue = nextLevelData.Value < 1f ? nextLevelData.Value * 100 : nextLevelData.Value;
            nextLevelText.SetString(nextLevelData.DetailStringID, nextLevelValue.ToString());
        }

    }
}
