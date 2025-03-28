using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitCanNormalAttackCondition : ConditionNode<Unit>
{
    public IsUnitCanNormalAttackCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsUnitCanAttack ? NodeStatus.Success : NodeStatus.Failure;
    }
}
