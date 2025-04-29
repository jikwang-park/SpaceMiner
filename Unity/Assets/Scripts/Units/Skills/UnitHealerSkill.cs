using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealerSkill : UnitSkillBase
{
    private HealerSkillTable.Data data;
    private Dictionary<UnitTypes, bool> hasBuffApplied = new Dictionary<UnitTypes, bool>();
    




    public void ResetDictionary()
    {
        hasBuffApplied.Clear();
    }
    public override void ExecuteSkill()
    {
        string soliderTarget = data.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = unit.StageManager.UnitPartyManager.GetCurrentTargetType(target);
            if(targetUnit is not null)
            {
                var amount = targetUnit.unitStats.maxHp * Ratio;
                targetUnit.unitStats.Hp += amount;
                ParticleEffectManager.Instance.PlayOneShot("HealEffect", targetUnit.transform);
            }
        }

        unit.lastSkillTime = Time.time;
    }

    public override void InitSkill(Unit unit)
    {
        this.unit = unit;
        skillId = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId[unit.UnitTypes][unit.Grade];
        data = DataTableManager.HealerSkillTable.GetData(skillId);
        skillGrade = unit.Grade;
        //GetBuff(skillId);
        CoolTime = data.CoolTime;
        Ratio = data.HealRatio;
    }


    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.HealerSkillTable.GetData(id);
        //GetBuff(id);
        this.data = data;
        CoolTime = data.CoolTime;
        Ratio = data.HealRatio;
    }

    //public override void GetBuff(int id)
    //{
    //    if(skillGrade == Grade.Legend)
    //    {
    //        var buffId = DataTableManager.HealerSkillTable.GetData(id).BuffID;
    //        buffData = DataTableManager.BuffTable.GetData(buffId);
    //    }
    //    else
    //    {
    //        buffData = null;
    //    }
    //}

    //public override void ExecuteBuff()
    //{
    //    string buffTarget = buffData.SoldierTarget;
    //    string[] targetStrings = buffTarget.Split("_");
    //    foreach (string target in targetStrings)
    //    {
    //        var targetUnit = unit.StageManager.UnitPartyManager.GetCurrentTargetType(target);
    //        if (targetUnit is null)
    //        {
    //            continue;
    //        }
    //        if(!hasBuffApplied.ContainsKey(targetUnit.UnitTypes)|| !hasBuffApplied[targetUnit.UnitTypes])
    //        {
    //            var takeDamage = targetUnit.GetComponent<AttackedTakeUnitDamage>();
    //            if (targetUnit.unitStats.Hp <= takeDamage.currentDamage)
    //            {
    //                var hpAmount = (targetUnit.unitStats.maxHp * buffData.RemainHP) / 100;
    //                targetUnit.unitStats.UseHealerBuff(hpAmount);

    //                hasBuffApplied[targetUnit.UnitTypes] = true;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("이미 버프가 적용되었던 유닛입니다.");
    //            return;
    //        }
    //    }
    //}

    
}