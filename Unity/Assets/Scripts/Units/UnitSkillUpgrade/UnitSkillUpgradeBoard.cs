using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillUpgradeBoard : MonoBehaviour
{
    private UnitSkillUpgradeElement upgradeElement;


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
        // upgradeElement.SetData();
    }
}
