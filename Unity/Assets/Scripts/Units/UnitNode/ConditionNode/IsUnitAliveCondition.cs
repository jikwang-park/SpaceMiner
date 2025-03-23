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
        Debug.Log("생존컨디션");
        if(!context.IsDead)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Failure;
    }
}
