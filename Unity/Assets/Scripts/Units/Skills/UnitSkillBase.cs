using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSkillBase
{
    
    public float CoolTime { get; protected set; }

    public float Ratio { get; protected set; }

    public List<Unit> targetList = new List<Unit>();

    protected abstract void ExcuteSkill();

    protected int skillId;

    protected Unit unit;

    public float remainCoolTime{get; protected set; }

    public abstract void InitSkill(Unit unit);

    public virtual void GetTarget() { }

    public abstract IEnumerator SkillRoutine();

    public abstract void UpdateCoolTime();



    public abstract void UpgradeUnitSkillStats(int id);
    
}


