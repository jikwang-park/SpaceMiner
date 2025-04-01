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
        var delaerSkillData = DataTableManager.DealerSkillTable.GetData(1101); //250331 HKY 데이터형 변경
        if (delaerSkillData != null )
        {
            coolTime = delaerSkillData.CoolTime;
            damageRatio = delaerSkillData.DamageRatio;
            targetCount = delaerSkillData.MonsterMaxTarget;
        }
    }


    public override void ExecuteSkill()
    {
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = stageManager.StageMonsterManager.GetMonsters(targetCount);
        for(int i = 0; i < targetTransforms.Count; ++i)
        {
            unit.unitStats.SkillExecute(targetTransforms[i].gameObject);
        }
    }

 
}
