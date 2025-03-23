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
        if(context.IsSkillUsing|| context.IsNormalAttack)
        {
            return NodeStatus.Failure;
        }
        return NodeStatus.Success;
    }
}
