using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitUsingSkillCondition : ConditionNode<Unit>
{
    public IsUnitUsingSkillCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsSkillUsing|| context.IsNormalAttacking)
        {
            return NodeStatus.Failure;
        }
        return NodeStatus.Success;
    }
}
