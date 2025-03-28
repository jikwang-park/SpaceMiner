using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerSkill : UnitSkill
{
    private float damageRatio;
    private int targetCount;

    protected override void Awake()
    {
        base.Awake();
        Init();
        unit = GetComponent<Unit>();
    }
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


    public override void ExecuteSkill()
    {
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = stageManager.MonsterLaneManager.GetMonsters(targetCount);
        for(int i = 0; i < targetTransforms.Count; ++i)
        {
            unit.unitStats.SkillExecute(targetTransforms[i].gameObject);
        }
    }

 
}
