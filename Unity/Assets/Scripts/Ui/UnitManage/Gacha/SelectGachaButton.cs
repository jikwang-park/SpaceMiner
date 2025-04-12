using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectGachaButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText;
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
        nameText.text = DataTableManager.StringTable.GetData(data.NameStringID); //250331 HKY 데이터형 변경
        gachaId = data.ID;
    }
    private void OnClickSelectButton()
    {
        if(parent != null)
        {
            parent.OnClickSelectGachaButton(gachaId);
        }
    }
}
