using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerSkill : UnitSkill
{


    private float shieldRatio;
    private float duration;
    private string buffId;
    private SkillType currentSkillType;
    


    private void Awake()
    {
    }

    

    public override void Init()
    {
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData("노말탱커스킬Lv1");
        if(tankerSkillData != null)
        {
            coolTime = tankerSkillData.CoolTime;
            shieldRatio = tankerSkillData.ShieldRatio;
            duration = tankerSkillData.Duration;
            buffId = tankerSkillData.BuffID;
        }
    }


     
    public override void SetTarget(List<Transform> target)
    {
        
    }
   
    public override void ExcuteSkill()
    {
        var buffTime = duration;

    }



}
