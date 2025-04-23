using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

public abstract class UnitSkillBase
{
    public float SkillCoolTimeRatio
    {
        get
        {
            if (unit.UnitStatus == Unit.Status.SkillUsing)
            {
                return 0f;
            }

            return Mathf.Min((Time.time - unit.lastSkillTime) / unit.Skill.CoolTime, 1f);
        }
    }
    public float CoolTime { get; protected set; }

    public float Ratio { get; protected set; }

    public List<Unit> targetList = new List<Unit>();

    public abstract void ExecuteSkill();

    protected int skillId;

    protected Unit unit;


    public float remainCoolTime{get; protected set; }

    public abstract void InitSkill(Unit unit);

    public virtual void GetTarget() { }


    public abstract void UpdateCoolTime();



    public abstract void UpgradeUnitSkillStats(int id);
    
}


