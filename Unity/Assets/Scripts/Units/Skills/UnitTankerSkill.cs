using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitTankerSkill : UnitSkillBase
{
    private TankerSkillTable.Data data;

    private float duration;

    public override void ExecuteSkill()
    {
        
        string soliderTarget = data.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = unit.StageManager.UnitPartyManager.GetCurrentTargetType(target);
            if (targetUnit is null)
            {
                continue;
            }
            var barrierAmount = unit.unitStats.armor * Ratio;
            targetUnit.unitStats.UseShiled(duration, barrierAmount);
            Debug.Log(barrierAmount);
        }
        if (skillGrade == Grade.Legend)
        {
            ExecuteBuff();
        }

        unit.lastSkillTime = Time.time;
    }



    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.Grade];
        data = DataTableManager.TankerSkillTable.GetData(skillId);
        skillGrade = unit.Grade;
        GetBuff(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
        duration = data.Duration;
    }

    public override void GetBuff(int id)
    {
        if(skillGrade == Grade.Legend)
        {
            var buffId = DataTableManager.TankerSkillTable.GetData(id).BuffID;
            buffData = DataTableManager.BuffTable.GetData(buffId);
        }
        else
        {
            buffData = null;
        }
        
    }
    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.TankerSkillTable.GetData(id);
        GetBuff(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
        duration = data.Duration;
    }

    public override void SetEtcValue(float value)
    {
        duration = value;
    }


    public override void ExecuteBuff()
    {
        string buffTarget = buffData.SoldierTarget;
        string[] targetStrings = buffTarget.Split("_");
        foreach(string target in targetStrings)
        {
            var targetUnit = unit.StageManager.UnitPartyManager.GetCurrentTargetType(target);
            if(targetUnit is null)
            {
                continue;
            }
            var buffAmount = (unit.unitStats.armor * buffData.Reflection)/100;
            targetUnit.unitStats.UseTankerBuff(buffData.Duration, buffAmount);
        }

    }

}
