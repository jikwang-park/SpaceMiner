using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMonsterSkillTargetExistCondition : ConditionNode<MonsterController>
{
    private MonsterSkill skill;

    public IsMonsterSkillTargetExistCondition(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override NodeStatus OnUpdate()
    {
        return skill.IsTargetExist ? NodeStatus.Success : NodeStatus.Failure;
    }
}
