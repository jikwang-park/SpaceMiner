using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerSkill : UnitSkill
{
    public float damageRatio { get; private set; }
    private int targetCount;

    protected override void Awake()
    {
        base.Awake();
        unit = GetComponent<Unit>();
    }


    public override void DealerInit(UnitTypes type, Grade grade)
    {
        currentType = type;
        currentSkillGrade = grade;

        var data = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        currentSkillId = data[currentType][currentSkillGrade];
        var dealerSkillData = DataTableManager.DealerSkillTable.GetData(currentSkillId); //250331 HKY 데이터형 변경
        if (dealerSkillData != null)
        {
            coolTime = dealerSkillData.CoolTime;
            damageRatio = dealerSkillData.DamageRatio;
            targetCount = dealerSkillData.MonsterMaxTarget;
        }
    }

 


    public override void ExecuteSkill()
    {
        base.ExecuteSkill();
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = stageManager.StageMonsterManager.GetMonsters(targetCount);
        for(int i = 0; i < targetTransforms.Count; ++i)
        {
            unit.unitStats.SkillExecute(targetTransforms[i].gameObject);
        }
    }

    public override void Update()
    {
        base.Update();
    }
    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.DealerSkillTable.GetData(id);
        coolTime = data.CoolTime;
        damageRatio = data.DamageRatio;
        targetCount = data.MonsterMaxTarget;
    }


}
