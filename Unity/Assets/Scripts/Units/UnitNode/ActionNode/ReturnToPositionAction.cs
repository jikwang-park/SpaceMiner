using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPositionAction : ActionNode<Unit>
{
    public ReturnToPositionAction(Unit context) : base(context)
    {
    }
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override NodeStatus OnUpdate()
    {
        if(!context.IsUnitFar)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Running;
    }
}
