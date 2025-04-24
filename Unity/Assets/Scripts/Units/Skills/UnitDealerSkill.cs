using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDealerSkill : UnitSkillBase
{
    private DealerSkillTable.Data data;

    public override void ExecuteSkill()
    {
        List<Transform> targetTransforms = new List<Transform>();
        targetTransforms = unit.StageManager.StageMonsterManager.GetMonsters(data.MonsterMaxTarget);
        for (int i = 0; i < targetTransforms.Count; ++i)
        {
            unit.unitStats.SkillExecute(targetTransforms[i].gameObject);
        }
        unit.lastSkillTime = Time.time;
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.Grade];
        data = DataTableManager.DealerSkillTable.GetData(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.DamageRatio;
    }



  



    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.DealerSkillTable.GetData(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.DamageRatio;
    }
}
