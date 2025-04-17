using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleAction : ActionNode<MonsterController>
{
    public MonsterIdleAction(MonsterController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();
        context.AnimationController.Play(AnimationControl.AnimationClipID.BattleIdle);
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.CanAttack
            || (context.hasTarget && context.CanMove))
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Running;
    }
}
