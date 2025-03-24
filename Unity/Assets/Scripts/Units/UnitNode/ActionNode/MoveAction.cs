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
        Debug.Log("�̵�����");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsUnitCanAttack)
        {
            Debug.Log("�̵�����");
            return NodeStatus.Failure;
        }
        Debug.Log("�̵���");
        context.Move();
        return NodeStatus.Running;
    }
}
