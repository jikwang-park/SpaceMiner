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
        Debug.Log("공격시작");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.currentStatus == Unit.UnitStatus.Attacking)
        {
            Debug.Log("공격중");
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
      
        
    }
}
