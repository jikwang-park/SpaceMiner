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
        context.IsNormalAttacking = true;
        context.lastAttackTime = Time.time;
        context.AttackCorutine();
        Debug.Log("공격시작");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsNormalAttacking)
        {
            Debug.Log("공격중");
            return NodeStatus.Running;
        }
        else
        {
            Debug.Log("공격성공");
            return NodeStatus.Success;
        }
      
        
    }
}
