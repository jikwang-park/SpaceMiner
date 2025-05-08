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

    private int dungeonType;

    public event System.Action<int> OnButtonClicked;

    public void SetType(int type)
    {
        dungeonType = type;
        var dungeonData = DataTableManager.DungeonTable.GetData(type, 1);
        nameText.SetString(dungeonData.NameStringID);
        detailText.SetString(dungeonData.DetailStringID);
        iconImage.SetSprite(dungeonData.SpriteID);
    }

    public void OnButtonClick()
    {
        OnButtonClicked?.Invoke(dungeonType);
    }
}
