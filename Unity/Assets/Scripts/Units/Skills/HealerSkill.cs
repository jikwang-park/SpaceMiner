using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill : UnitSkill
{
    private float healRatio;
    private int buffId; //250331 HKY 데이터형 변경

    protected override void Awake()
    {
        base.Awake();
        unit = GetComponent<Unit>();
       
    }

    private void Start()
    {
         
    }

    public override void HealerInit(UnitTypes type, Grade grade)
    {
        currentType = type;
        currentSkillGrade = grade;

        var data = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        currentSkillId = data[currentType][currentSkillGrade];
        var healerSkillData = DataTableManager.HealerSkillTable.GetData(currentSkillId); //250331 HKY 데이터형 변경
        if (healerSkillData != null)
        {
            coolTime = healerSkillData.CoolTime;
            healRatio = healerSkillData.HealRatio;
            buffId = healerSkillData.BuffID;
            Debug.Log(healRatio);
        }
        GetTarget();
    }
   

    public override void GetTarget()
    {
        var healerSkillData = DataTableManager.HealerSkillTable.GetData(currentSkillId); //250331 HKY 데이터형 변경
        string soliderTarget = healerSkillData.SoldierTarget;
        string[] targetStrings = soliderTarget.Split("_");
        foreach (string target in targetStrings)
        {
            var targetUnit = stageManager.UnitPartyManager.GetCurrentTargetType(target);

            targetList.Add(targetUnit);
        }
    }

    public override void ExecuteSkill()
    {
        foreach(var target in targetList)
        {
            var amount = unit.unitStats.maxHp * healRatio;
            unit.unitStats.Hp += amount;
        }
    }

    public override void UpgradeUnitSkillStats(int id)
    {
        var data = DataTableManager.HealerSkillTable.GetData(id);
        coolTime = data.CoolTime;
        healRatio = data.HealRatio;
        buffId = data.BuffID;
    }


}
