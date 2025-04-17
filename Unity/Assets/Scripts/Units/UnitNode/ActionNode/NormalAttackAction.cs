using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackAction : ActionNode<Unit>
{
  
    public NormalAttackAction(Unit context) : base(context)
    {

    }
    protected override void OnStart()
    {
        base.OnStart();
        context.AttackCorutine();
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.currentStatus == Unit.UnitStatus.Attacking)
        {
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
      
        
    }
}
