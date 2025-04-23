using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealerSkill : UnitSkillBase
{
    private HealerSkillTable.Data data;

    public override void ExecuteSkill()
    {
        GetTarget();
        foreach (var target in targetList)
        {
            var amount = target.unitStats.maxHp * data.HealRatio;
            target.unitStats.Hp += amount;
            Debug.Log(amount);
        }
        remainCoolTime = CoolTime;
        unit.lastSkillTime = Time.time;
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.Grade];
        data = DataTableManager.HealerSkillTable.GetData(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.HealRatio;
        remainCoolTime = 0f;
    }

    public override void GetTarget()
    {
        string soliderTarget = data.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = unit.StageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }

    


    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.HealerSkillTable.GetData(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.HealRatio;
        GetTarget();
    }

    public override void UpdateCoolTime()
    {
        if (remainCoolTime > 0)
        {
            remainCoolTime -= Time.deltaTime;
            remainCoolTime = Mathf.Max(remainCoolTime, 0f);
        }
    }
}