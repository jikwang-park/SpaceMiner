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
        // 처음 스킬 업그레이드 버튼 클릭했을시에 탱커 -> 노말 기준으로 뜨게 여기서 설정해야됌

    }
    public void ShowBoard()
    {
        // upgradeElement.SetData();
    }
}
