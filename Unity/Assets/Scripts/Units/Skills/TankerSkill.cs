using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerSkill : UnitSkill
{
    private float shieldRatio;
    private float duration;
    private string buffId;
    
    private SkillType currentSkillType;
    private List<Unit> targetList;    

    // init 贸府 秦拎具达
    public override void Init()
    {
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData("畴富攀目胶懦Lv1");
        if(tankerSkillData != null)
        {
            coolTime = tankerSkillData.CoolTime;
            shieldRatio = tankerSkillData.ShieldRatio;
            duration = tankerSkillData.Duration;
            buffId = tankerSkillData.BuffID;
            GetTarget();
        }
    }   
    
    public void GetTarget()
    {
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData("畴富攀目胶懦Lv1");
        string soliderTarget = tankerSkillData.SoilderTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach(string target in targetStrings)
        {
            var targetUnit = stageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }
    private void Update()
    {

    }

    public override void ExcuteSkill()
    {
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.armor * shieldRatio;
            target.SetBarrier(duration, amount);
        }
    }
    
}
