using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitSkillUpgradeBoard : MonoBehaviour
{
    private UnitSkillUpgradeManager manager;

    private UnitSkillUpgradeElement element;

    [SerializeField]
    private Image currentImage;
    [SerializeField]
    private TextMeshProUGUI currentText;
    [SerializeField]
    private Image nextImage;
    [SerializeField]
    private TextMeshProUGUI nextText;


    private int id;
    private void Start()
    {
    }
    public void ShowFirstOpened()
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(1);
        // ó�� ��ų ���׷��̵� ��ư Ŭ�������ÿ� ��Ŀ -> �븻 �������� �߰� ���⼭ �����ؾ߉�

    }
    public void ShowBoard()
    {

    }

    public void SetInfo(int id)
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(id);
    }

    public void SetBoardText(int id)
    {
        var data = DataTableManager.SkillUpgradeTable.GetData(id);

        currentText.text = $"";
        
    }

    public void SetImage()
    {

    }
}
