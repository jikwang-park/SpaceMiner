using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill : UnitSkill
{
    private float healRatio;
    private int buffId; //250331 HKY 데이터형 변경

    protected override void Awake()
    {
        base.Awake();
        Init();
        unit = GetComponent<Unit>();
    }
    public override void Init()
    {
        var healerSkillData = DataTableManager.HealerSkillTable.GetData(1301); //250331 HKY 데이터형 변경
        if (healerSkillData != null)
        {
            coolTime = healerSkillData.CoolTime;
            healRatio = healerSkillData.HealRatio;
            buffId = healerSkillData.BuffID;
        }
    }

    public override void GetTarget()
    {
        var tankerSkillData = DataTableManager.HealerSkillTable.GetData(1301); //250331 HKY 데이터형 변경
        string soliderTarget = tankerSkillData.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = stageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }

    public override void ExecuteSkill()
    {
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.maxHp * healRatio;
            unit.unitStats.Hp += amount;
        }
    }

   
}
