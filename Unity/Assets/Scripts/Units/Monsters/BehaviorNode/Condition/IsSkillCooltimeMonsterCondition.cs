using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSkillCooltimeMonsterCondition : ConditionNode<MonsterController>
{
    private MonsterSkill skill;

    public IsSkillCooltimeMonsterCondition(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override NodeStatus OnUpdate()
    {
        return skill.IsCoolTime ? NodeStatus.Success : NodeStatus.Failure;
    }
}
