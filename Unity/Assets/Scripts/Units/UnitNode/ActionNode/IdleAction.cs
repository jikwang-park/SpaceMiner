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
        Debug.Log(context.UnitTypes);
    }
    protected override NodeStatus OnUpdate()
    {
        if(context.targetPos == null)
        {
            return NodeStatus.Failure;
        }
        if (context.IsSkillCoolTimeOn || context.IsAttackCoolTimeOn)
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Running;
    }
}
