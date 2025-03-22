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
        context.IsNormalAttack = true;
        context.lastAttackTime = Time.time;
        context.AttackCorutine();
        Debug.Log("���ݽ���");
    }

    protected override NodeStatus OnUpdate()
    {
        if(context.IsNormalAttack)
        {
            Debug.Log("������");
            return NodeStatus.Running;
        }
        else
        {
            context.IsNormalAttack = false;
            Debug.Log("���ݼ���");
            context.lastAttackTime = Time.time;
            return NodeStatus.Success;
        }
      
        
    }
}
