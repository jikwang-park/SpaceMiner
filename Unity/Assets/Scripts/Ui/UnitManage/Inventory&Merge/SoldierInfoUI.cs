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
    private TextMeshProUGUI attackAmount;
    [SerializeField]
    private TextMeshProUGUI armorAmount;
    [SerializeField]
    private TextMeshProUGUI hpAmount;
    [SerializeField]
    private Button equipButton;

    public void Initialize(InventoryElement element)
    {
        var sprite = element.GetComponent<Image>().sprite;
        var data = DataTableManager.SoldierTable.GetData(element.soldierId);
        soldierInfo.Initialize(element.Grade, element.Level, element.Count.ToString(), sprite, data.SpriteID);
        attackAmount.text = $"���ݷ�: {(BigNumber)data.Attack}";
        armorAmount.text = $"����: {(BigNumber)data.Defence}";
        hpAmount.text = $"ü��: {(BigNumber)data.HP}";
    }
}
