using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHealerCanUseSkillCondition : ConditionNode<Unit>
{
    public IsHealerCanUseSkillCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsHealerCanUseSkill ? NodeStatus.Success : NodeStatus.Failure;
    }
}
