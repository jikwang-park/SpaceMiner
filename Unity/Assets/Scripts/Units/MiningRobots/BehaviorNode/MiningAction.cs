using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAction : ActionNode<MiningRobotController>
{
    private float targetTime;
    private AnimationControl animatorControl;

    public MiningAction(MiningRobotController context) : base(context)
    {
        animatorControl = context.GetComponent<AnimationControl>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        switch (context.Slot)
        {
            case 0:
                targetTime = Time.time + (float)context.PlanetData.MiningLevel1 / context.RobotData.MiningSpeed;
                break;
            case 1:
                targetTime = Time.time + (float)context.PlanetData.MiningLevel2 / context.RobotData.MiningSpeed;
                break;
        }
        animatorControl.Play(AnimationControl.AnimationClipID.Attack);
    }

    protected override NodeStatus OnUpdate()
    {
        if (Time.time < targetTime)
        {
            return NodeStatus.Running;
        }
        context.ChangeTarget(false);
        return NodeStatus.Success;
    }
}
