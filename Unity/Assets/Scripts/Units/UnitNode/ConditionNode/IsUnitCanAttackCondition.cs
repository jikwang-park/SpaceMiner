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
        Debug.Log("canattack");
        if (context.IsAttackCoolTimeOn)
            return NodeStatus.Success;

        return NodeStatus.Failure;
    }
}
