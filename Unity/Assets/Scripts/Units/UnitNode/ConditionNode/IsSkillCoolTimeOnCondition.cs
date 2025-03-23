
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSkillCoolTimeOnCondition : ConditionNode<Unit>
{
    public IsSkillCoolTimeOnCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.IsSkillCoolTimeOn)
            return NodeStatus.Success;

        return NodeStatus.Failure;
    }
}
