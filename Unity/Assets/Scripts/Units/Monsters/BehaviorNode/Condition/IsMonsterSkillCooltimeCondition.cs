using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMonsterSkillCooltimeCondition : ConditionNode<MonsterController>
{
    private MonsterSkill skill;

    public IsMonsterSkillCooltimeCondition(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override NodeStatus OnUpdate()
    {
        return skill.IsCoolTime ? NodeStatus.Success : NodeStatus.Failure;
    }
}
