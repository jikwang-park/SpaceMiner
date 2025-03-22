using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitHitCondition : ConditionNode<Unit>
{
    public IsUnitHitCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsUnitHit)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
