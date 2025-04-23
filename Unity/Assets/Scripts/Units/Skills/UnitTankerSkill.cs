using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTankerSkill : UnitSkillBase
{
    private TankerSkillTable.Data data;

    protected override void ExcuteSkill()
    {
        GetTarget();
        foreach (var target in targetList)
        {
            var barrierAmount = unit.unitStats.armor * Ratio;
            target.SetBarrier(data.Duration, barrierAmount);
        }
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.currentGrade];
        data = DataTableManager.TankerSkillTable.GetData(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
        remainCoolTime = 0f;
    }

    public override void GetTarget()
    {
        string soliderTarget = data.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = unit.stageManager.UnitPartyManager.GetCurrentTargetType(target);
            targetList.Add(targetUnit);
        }
    }

    public override IEnumerator SkillRoutine()
    {
        ExcuteSkill();
        remainCoolTime = CoolTime;
        unit.currentStatus = Unit.UnitStatus.Wait;
        unit.lastSkillUsedTime = Time.time;
        yield return new WaitForSeconds(0.25f);
    }


    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.TankerSkillTable.GetData(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.ShieldRatio;
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
