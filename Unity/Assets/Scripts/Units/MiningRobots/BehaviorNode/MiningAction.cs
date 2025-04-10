using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAction : ActionNode<MiningRobotController>
{
    private float targetTime;

    public MiningAction(MiningRobotController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();

        switch (context.Slot)
        {
            case 0:
                targetTime = Time.time + (float)context.PlanetData.miningLevel / context.RobotData.miningSpeed;
                break;
            case 1:
                targetTime = Time.time + (float)context.PlanetData.miningLevel2 / context.RobotData.miningSpeed;
                break;
        }
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
