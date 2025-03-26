using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerSkill : UnitSkill
{
    private float damageRatio;
    private SkillType currentSkillType;
    private int targetCount;
     

    public override void Init()
    {
        var delaerSkillData = DataTableManager.DealerSkillTable.GetData("노말딜러스킬Lv1");
        if(delaerSkillData != null )
        {
            coolTime = delaerSkillData.CoolTime;
            damageRatio = delaerSkillData.DamageRatio;
            targetCount = delaerSkillData.MonsterMaxTarget;
        }
    }


    public override void ExcuteSkill()
    {
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = stageManager.MonsterLaneManager.GetMonsters(targetCount);



    }

 
}
