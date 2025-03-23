using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitNotOnRangeCondition : ConditionNode<Unit>
{
    public IsUnitNotOnRangeCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        if (!context.IsUnitCanAttack)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
