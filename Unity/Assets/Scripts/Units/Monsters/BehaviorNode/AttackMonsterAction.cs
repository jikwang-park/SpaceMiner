using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMonsterAction : ActionNode<MonsterController>
{
    public AttackMonsterAction(MonsterController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
        context.AttackTarget();
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.status == MonsterController.Status.Attacking)
        {
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
    }
}
