using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitFarCondition : ConditionNode<Unit>
{
    public IsUnitFarCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsUnitFar ? NodeStatus.Success : NodeStatus.Failure;
    }
}
