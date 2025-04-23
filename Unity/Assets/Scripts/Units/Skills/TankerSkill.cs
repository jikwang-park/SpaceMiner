using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerSkill : UnitSkill
{
    private float shieldRatio;
    private int buffId; //250331 HKY 데이터형 변경

    
    protected override void Awake()
    {
        base.Awake();
        currentType = UnitTypes.Tanker;
        unit = GetComponent<Unit>();
    }

    
    public override void TankerInit(UnitTypes type , Grade grade)
    {
        currentType = type;
        currentSkillGrade = grade;
      
        var data = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        currentSkillId = data[currentType][currentSkillGrade];
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData(currentSkillId); //250331 HKY 데이터형 변경
        if (tankerSkillData != null)
        {
            coolTime = tankerSkillData.CoolTime;
            shieldRatio = tankerSkillData.ShieldRatio;
            duration = tankerSkillData.Duration;
            buffId = tankerSkillData.BuffID;

        }
    }


    public override void GetTarget()
    {
        var tankerSkillData = DataTableManager.TankerSkillTable.GetData(currentSkillId); //250331 HKY 데이터형 변경
        string soliderTarget = tankerSkillData.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach(string target in targetStrings)
        {
            var targetUnit = stageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }
  

    public override void ExecuteSkill()
    {
        base.ExecuteSkill();
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.armor * shieldRatio;
            //target.SetBarrier(duration, amount);
        }
    }
    public override void Update()
    {
        base.Update();
    }

    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.TankerSkillTable.GetData(id);
        coolTime = data.CoolTime;
        shieldRatio = data.ShieldRatio;
        duration = data.Duration;
        buffId = data.BuffID;
    }

}
