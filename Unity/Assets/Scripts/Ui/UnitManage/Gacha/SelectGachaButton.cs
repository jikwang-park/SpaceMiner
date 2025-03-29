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
    public string gachaId { get; private set; }
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
        nameText.text = data.nameStringID;
        gachaId = data.gachaID;
    }
    private void OnClickSelectButton()
    {
        if(parent != null)
        {
            parent.OnClickSelectGachaButton(gachaId);
        }
    }
}
