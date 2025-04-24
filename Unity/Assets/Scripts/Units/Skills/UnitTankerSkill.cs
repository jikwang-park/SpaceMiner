using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTankerSkill : UnitSkillBase
{
    private TankerSkillTable.Data data;

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
            targetUnit.unitStats.UseShiled(data.Duration, barrierAmount);
        }

        unit.lastSkillTime = Time.time;
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.Grade];
        data = DataTableManager.TankerSkillTable.GetData(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
    }


    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.TankerSkillTable.GetData(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
    }
}
