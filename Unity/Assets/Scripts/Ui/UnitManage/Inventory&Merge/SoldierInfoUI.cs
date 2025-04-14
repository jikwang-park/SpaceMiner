using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInfoUI : MonoBehaviour
{
    [SerializeField]
    private SoldierInfoImage soldierInfo;
    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Button equipButton;

    public void Initialize(InventoryElement element)
    {
        var sprite = element.GetComponent<Image>().sprite;
        var data = DataTableManager.SoldierTable.GetData(element.soldierId);
        soldierInfo.Initialize(element.Level.ToString(), element.Count.ToString(), sprite);
        gradeText.text = data.Grade.ToString();
        nameText.text = DataTableManager.StringTable.GetData(data.NameStringID);
    }
}
