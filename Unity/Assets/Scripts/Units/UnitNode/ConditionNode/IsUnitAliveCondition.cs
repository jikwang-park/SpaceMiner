using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitAliveCondition : ConditionNode<Unit>
{
    public IsUnitAliveCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return !context.IsDead ? NodeStatus.Success : NodeStatus.Failure;
    }
}
