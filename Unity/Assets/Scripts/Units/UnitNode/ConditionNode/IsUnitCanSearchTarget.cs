using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitCanSearchTarget : ConditionNode<Unit>
{
    public IsUnitCanSearchTarget(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.targetPos == null)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
