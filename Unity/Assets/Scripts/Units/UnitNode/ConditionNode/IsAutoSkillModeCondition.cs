using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAutoSkillModeCondition : ConditionNode<Unit>
{
    public IsAutoSkillModeCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.isAutoSkillMode ? NodeStatus.Success : NodeStatus.Failure;
    }
}
