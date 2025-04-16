using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectGachaButton : MonoBehaviour
{
    [SerializeField]
    private LocalizationText nameText;
    [SerializeField]
    private AddressableImage icon;
    public int gachaId { get; private set; } //250331 HKY 데이터형 변경
    private Button button;
    public GachaInteractableUI parent;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(() => OnClickSelectButton());
    }

    public void Initialize(GachaTable.Data data)
    {
        gachaId = data.ID;
        nameText.SetString(data.NameStringID);
        icon.SetSprite(DataTableManager.ItemTable.GetData(data.NeedItemID1).SpriteID);
    }
    private void OnClickSelectButton()
    {
        if(parent != null)
        {
            parent.OnClickSelectGachaButton(gachaId);
        }
    }
}
