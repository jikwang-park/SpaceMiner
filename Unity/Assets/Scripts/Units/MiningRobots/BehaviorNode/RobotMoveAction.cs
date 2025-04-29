using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMoveAction : ActionNode<MiningRobotController>
{
    private Vector3 direction;
    private AnimationControl animatorControl;

    public RobotMoveAction(MiningRobotController context) : base(context)
    {
        animatorControl = context.GetComponent<AnimationControl>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        direction = (context.currentTarget.position - context.transform.position).normalized;
        switch (context.RobotData.Grade)
        {
            case Grade.Normal:
            case Grade.Rare:
                animatorControl.Play(AnimationControl.AnimationClipID.Walk);
                break;
            case Grade.Epic:
            case Grade.Legend:
                animatorControl.Play(AnimationControl.AnimationClipID.Run);
                break;
        }
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.sqrDistance > 0.001f
            && Vector3.Dot(direction, context.currentTarget.position - context.transform.position) >= 0f)
        {
            var moving = Time.deltaTime * direction * context.Speed;
            if (context.sqrDistance > moving.sqrMagnitude)
            {
                context.transform.position += moving;
                context.transform.rotation = Quaternion.Lerp(context.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * context.Speed);
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
