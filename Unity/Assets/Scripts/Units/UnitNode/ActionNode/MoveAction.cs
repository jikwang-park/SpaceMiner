using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : ActionNode<Unit>
{
    public MoveAction(Unit context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsUnitCanAttack)
        {
            return NodeStatus.Failure;
        }
        context.Move();
        return NodeStatus.Running;
    }
}
