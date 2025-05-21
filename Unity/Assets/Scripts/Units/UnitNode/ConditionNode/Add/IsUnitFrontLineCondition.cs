using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitFrontLineCondition : ConditionNode<Unit>
{
    public IsUnitFrontLineCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsFrontLine ? NodeStatus.Success : NodeStatus.Failure;
    }
}
