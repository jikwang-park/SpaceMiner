using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackAction : ActionNode<Unit>
{

    public SkillAttackAction(Unit context) : base(context)
    {
    }
    protected override void OnStart()
    {
        base.OnStart();
        context.UseSkill();
        Debug.Log("��ų ������");
    }
    protected override NodeStatus OnUpdate()
    {
        if (context.currentStatus == Unit.UnitStatus.UsingSkill)
        {
            Debug.Log("��ų�����");
            return NodeStatus.Running;

        }

        return NodeStatus.Success;

    }
}
