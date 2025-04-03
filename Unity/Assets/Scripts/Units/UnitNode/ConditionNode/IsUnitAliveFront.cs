using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitAliveFront : ConditionNode<Unit>
{
    public IsUnitAliveFront(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
       return context.IsUnitAliveFront ? NodeStatus.Success : NodeStatus.Failure;
    }
}
