using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitCanAttackCondition : ConditionNode<Unit>
{
    public IsUnitCanAttackCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsUnitCanAttack ? NodeStatus.Success : NodeStatus.Failure;
    }
}
