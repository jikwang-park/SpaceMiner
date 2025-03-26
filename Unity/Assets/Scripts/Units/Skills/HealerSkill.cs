using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill : UnitSkill
{
    private float healRatio;
    private string buffId;

    private List<Unit> targetList;

    public override void Init()
    {
        var healerSkillData = DataTableManager.HealerSkillTable.GetData("노말힐러스킬Lv1");
        if (healerSkillData != null)
        {
            coolTime = healerSkillData.CoolTime;
            healRatio = healerSkillData.HealRatio;
            buffId = healerSkillData.BuffID;
            GetTarget();
        }
    }

    public void GetTarget()
    {
        var tankerSkillData = DataTableManager.HealerSkillTable.GetData("노말힐러스킬Lv1");
        string soliderTarget = tankerSkillData.SoilderTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = stageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }

    public override void ExcuteSkill()
    {
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.maxHp * healRatio;
            unit.unitStats.Hp += amount;
        }
    }

   
}
