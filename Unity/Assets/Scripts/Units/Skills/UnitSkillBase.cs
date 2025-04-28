using System.Collections;
using System.Collections.Generic;
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

    public abstract void ExecuteSkill();

    protected int skillId;

    protected Unit unit;


    public float RemainCooltime
    {
        get
        {
            if (unit.UnitStatus == Unit.Status.SkillUsing)
            {
                return CoolTime;
            }
            return Mathf.Max(CoolTime + unit.lastSkillTime - Time.time, 0f);
        }
    }

    public abstract void InitSkill(Unit unit);




    public abstract void UpgradeUnitSkillStats(int id);
    
    public void SetCoolTime(float cooltime)
    {
        CoolTime = cooltime;
    }

    public void SetRatio(float ratio)
    {
        Ratio = ratio;
    }

    public virtual void SetEtcValue(float value)
    {

    }
}


