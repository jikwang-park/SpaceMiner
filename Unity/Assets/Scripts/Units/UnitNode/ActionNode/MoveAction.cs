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
        Debug.Log("이동시작");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsUnitCanAttack)
        {
            Debug.Log("이동종료");
            return NodeStatus.Failure;
        }
        Debug.Log("이동중");
        context.Move();
        return NodeStatus.Running;
    }
}
