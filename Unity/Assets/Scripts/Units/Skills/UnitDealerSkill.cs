using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDealerSkill : UnitSkillBase
{
    private DealerSkillTable.Data data;

    protected override void ExcuteSkill()
    {
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = unit.stageManager.StageMonsterManager.GetMonsters(data.MonsterMaxTarget);
        for (int i = 0; i < targetTransforms.Count; ++i)
        {
            unit.unitStats.SkillExecute(targetTransforms[i].gameObject);
        }
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.currentGrade];
        data = DataTableManager.DealerSkillTable.GetData(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.DamageRatio;
        remainCoolTime = 0f;
    }



    public override IEnumerator SkillRoutine()
    {
        remainCoolTime = CoolTime;
        ExcuteSkill();
        unit.currentStatus = Unit.UnitStatus.Wait;
        unit.lastSkillUsedTime = Time.time;
        yield return new WaitForSeconds(0.25f);
    }



    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.DealerSkillTable.GetData(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.DamageRatio;
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
