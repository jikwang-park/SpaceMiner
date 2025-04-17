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
    }
    protected override NodeStatus OnUpdate()
    {
        if (context.currentStatus == Unit.UnitStatus.UsingSkill)
        {
            return NodeStatus.Running;

        }

        return NodeStatus.Success;

    }
}
