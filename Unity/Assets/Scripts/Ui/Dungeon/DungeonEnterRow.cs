using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonEnterRow : MonoBehaviour
{
    [SerializeField]
    private LocalizationText nameText;
    [SerializeField]
    private LocalizationText detailText;
    [SerializeField]
    private AddressableImage iconImage;
    [SerializeField]
    private LocalizationText keyAmountText;

    private int dungeonType;

    public event System.Action<int> OnButtonClicked;

    private int keyItemID;
    private BigNumber needItemCount;

    public void SetType(int type)
    {
        dungeonType = type;
        var dungeonData = DataTableManager.DungeonTable.GetData(type, 1);
        keyItemID = dungeonData.NeedKeyItemID;
        needItemCount = dungeonData.NeedKeyItemCount;
        nameText.SetString(dungeonData.NameStringID);
        detailText.SetString(dungeonData.NameStringID);
        iconImage.SetSprite(dungeonData.SpriteID);
        keyAmountText.SetStringArguments(needItemCount.ToString(), ItemManager.GetItemAmount(dungeonData.NeedKeyItemID).ToString());

        ItemManager.OnItemAmountChanged += OnItemAmountChanged;
    }

    private void OnItemAmountChanged(int itemId, BigNumber amount)
    {
        if (keyItemID != itemId)
        {
            return;
        }
        keyAmountText.SetStringArguments(needItemCount.ToString(), amount.ToString());

    }

    public void OnButtonClick()
    {
        OnButtonClicked?.Invoke(dungeonType);
    }
}
