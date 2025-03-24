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
        return context.targetPos == null ? NodeStatus.Success : NodeStatus.Failure;
    }
}
