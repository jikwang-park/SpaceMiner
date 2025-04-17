using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterIdleAction : ActionNode<MonsterController>
{
    public MonsterIdleAction(MonsterController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (context.AnimationFound)
        {
            context.AnimationController.Play(AnimationControl.AnimationClipID.BattleIdle);
        }
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.CanAttack
            || (context.TargetAcquired && context.CanMove))
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Running;
    }
}
