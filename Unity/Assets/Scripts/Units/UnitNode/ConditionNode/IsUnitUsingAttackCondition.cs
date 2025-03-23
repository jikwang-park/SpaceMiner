using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitUsingAttackCondition : ConditionNode<Unit>
{
    public IsUnitUsingAttackCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.IsAttackCoolTimeOn)
            return NodeStatus.Success;

        return NodeStatus.Failure;
    }


}
