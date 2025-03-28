using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerSkill : UnitSkill
{
    private float shieldRatio;
    private string buffId;


    protected override void Awake()
    {
        base.Awake();
        Init();
        unit = GetComponent<Unit>();
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
    
    public override void GetTarget()
    {
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData("노말탱커스킬Lv1");
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

    public override void ExecuteSkill()
    {
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.armor * shieldRatio;
            target.SetBarrier(duration, amount);
        }
    }
    
}
