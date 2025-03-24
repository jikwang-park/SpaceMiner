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
        context.AttackCorutine();
        Debug.Log("���ݽ���");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsNormalAttacking)
        {
            Debug.Log("������");
            return NodeStatus.Running;
        }
        else
        {
            context.IsNormalAttacking = false;
            Debug.Log("���ݼ���");
            context.lastAttackTime = Time.time;
            return NodeStatus.Success;
        }
      
        
    }
}
