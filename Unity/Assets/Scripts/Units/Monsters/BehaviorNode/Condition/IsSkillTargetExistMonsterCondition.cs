using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSkillTargetExistMonsterCondition : ConditionNode<MonsterController>
{
    private MonsterSkill skill;

    public IsSkillTargetExistMonsterCondition(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override NodeStatus OnUpdate()
    {
        return skill.IsTargetExist ? NodeStatus.Success : NodeStatus.Failure;
    }
}
