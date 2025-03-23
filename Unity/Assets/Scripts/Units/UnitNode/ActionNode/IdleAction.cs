using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionNode<Unit>
{
    
    public IdleAction(Unit context) : base(context)
    {
    }
    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log("대기시작");
    }
    protected override NodeStatus OnUpdate()
    {
        if (context.IsSkillCoolTimeOn || context.IsAttackCoolTimeOn)
        {
            return NodeStatus.Failure;
        }

        return NodeStatus.Running;
    }
}
