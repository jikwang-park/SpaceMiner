using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackAction : ActionNode<Unit>
{
    private float lastAttackTime = 0f;

    public SkillAttackAction(Unit context) : base(context)
    {
    }
    protected override void OnStart()
    {
        base.OnStart();
        context.lastSkillAttackTime = Time.time;
        context.IsSkillUsing = true;
        Debug.Log("��ų ������");
    }
    protected override NodeStatus OnUpdate()
    {
        if(context.IsSkillUsing)
        {

            return NodeStatus.Running;
        }
        else
        {
            Debug.Log("��ų ��뼺��");
            context.IsSkillUsing = false;
            return NodeStatus.Success;
        }
       
    }
}
