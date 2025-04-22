using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetInRangeMonsterCondition : ConditionNode<MonsterController>
{
    public IsTargetInRangeMonsterCondition(MonsterController context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsTargetInRange ? NodeStatus.Success : NodeStatus.Failure;
    }
}
