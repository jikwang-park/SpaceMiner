using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitAutoMode : ConditionNode<Unit>
{
    public IsUnitAutoMode(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.isAutoSkillMode ? NodeStatus.Success : NodeStatus.Failure;
    }
}
