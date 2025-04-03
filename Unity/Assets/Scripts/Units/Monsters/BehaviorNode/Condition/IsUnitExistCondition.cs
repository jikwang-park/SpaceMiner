using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitExistCondition : ConditionNode<MonsterController>
{
    public IsUnitExistCondition(MonsterController context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.TargetAcquired ? NodeStatus.Success : NodeStatus.Failure;
    }
}
