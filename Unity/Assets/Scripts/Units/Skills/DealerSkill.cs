using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerSkill : UnitSkill
{
    private float damageRatio;
    private SkillType currentSkillType;
    private 

    public override void Init()
    {
        var delaerSkillData = DataTableManager.DealerSkillTable.GetData("�븻������ųLv1");
        if(delaerSkillData != null )
        {
            coolTime = delaerSkillData.CoolTime;
            damageRatio = delaerSkillData.DamageRatio;
        }
    }


    public override void ExcuteSkill()
    {
        throw new System.NotImplementedException();
    }

 
}
