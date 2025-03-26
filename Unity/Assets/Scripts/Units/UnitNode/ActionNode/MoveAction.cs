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
        if (context.targetPos != null || context.targetDistance < context.unitStats.range)
        {

            Debug.Log("이동종료");
            return NodeStatus.Success;
        }
        Debug.Log("이동중");
        context.Move();

        return NodeStatus.Running;
    }
}
