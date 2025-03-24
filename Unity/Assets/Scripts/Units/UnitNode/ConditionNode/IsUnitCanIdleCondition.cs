using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitCanIdleCondition : ConditionNode<Unit>
{
    public IsUnitCanIdleCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if (!context.IsAttackCoolTimeOn && context.targetPos != null)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
