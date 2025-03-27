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
        Debug.Log("스킬 사용시작");
    }
    protected override NodeStatus OnUpdate()
    {
        if (context.currentStatus == Unit.UnitStatus.UsingSkill)
        {
            Debug.Log("스킬사용중");
            return NodeStatus.Running;

        }

        return NodeStatus.Success;

    }
}
