using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectItemElement : MonoBehaviour
{
    [SerializeField]
    private EffectItemIcon icon;
    [SerializeField]
    private AmountSlider amountSlider;
    [SerializeField]
    private Image alarmImage;
    
    public int level = 0;

    public EffectItemTable.ItemType type;
    public EffectItemInventory parentInventory;
    public void Initialize(EffectItemTable.ItemType type)
    {
        this.type = type;
        level = EffectItemInventoryManager.GetLevel(type);

        icon.Initialize(this.type, level);

        var currentLevelData = DataTableManager.EffectItemTable.GetDatas(type)[level];

        int needItemId = currentLevelData.NeedItemID;
        int requireLevelUpAmount = currentLevelData.NeedItemCount;

        amountSlider.SetValue(int.Parse(ItemManager.GetItemAmount(needItemId).ToString()), requireLevelUpAmount);
    }
    private void OnDisable()
    {
        if(alarmImage.gameObject.activeSelf)
        {
            alarmImage.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        if(type != 0)
        {
            UpdateElement();
        }
    }
    public void UpdateElement()
    {
        int newLevel = EffectItemInventoryManager.GetLevel(type);
        if (level == newLevel)
        {
            return;
        }

        if(level == 0 && newLevel != 0)
        {
            Unlock();
        }
        Initialize(type);
    }
    private void Unlock()
    {
        alarmImage.gameObject.SetActive(true);
    }

    public void OnClickEffectItem()
    {
        parentInventory.OnClickEffectItem(this);
    }
}
