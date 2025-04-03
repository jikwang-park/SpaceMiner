using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitInSafeDistanceCondition : ConditionNode<Unit>
{
    public IsUnitInSafeDistanceCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsSafeDistance ? NodeStatus.Success : NodeStatus.Failure;
    }
}
