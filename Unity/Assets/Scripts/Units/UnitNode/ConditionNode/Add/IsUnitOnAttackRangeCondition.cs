using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitOnAttackRangeCondition : ConditionNode<Unit>
{
    public IsUnitOnAttackRangeCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
    }
}
