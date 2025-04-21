using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitExceedMonsterPosCondition : ConditionNode<Unit>
{
    public IsUnitExceedMonsterPosCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsUnitExeedMonsterPosition ? NodeStatus.Success : NodeStatus.Failure;
    }
}
