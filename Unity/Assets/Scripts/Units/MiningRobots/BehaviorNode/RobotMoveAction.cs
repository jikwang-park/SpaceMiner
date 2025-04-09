using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMoveAction : ActionNode<MiningRobotController>
{
    private Vector3 direction;


    public RobotMoveAction(MiningRobotController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
        direction = (context.currentTarget.position - context.transform.position).normalized;
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.sqrDistance > 0.001f)
        {
            var moving = Time.deltaTime * direction * context.RobotData.moveSpeed;
            if (context.sqrDistance > moving.sqrMagnitude)
            {
                context.transform.position += moving;
                return NodeStatus.Running;
            }
            else
            {
                context.transform.position = context.currentTarget.position;
            }
        }

        return NodeStatus.Success;
    }
}
