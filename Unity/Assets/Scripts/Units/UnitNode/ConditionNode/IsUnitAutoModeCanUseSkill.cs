using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsUnitAutoModeCanUseSkill : ConditionNode<Unit>
{
    public IsUnitAutoModeCanUseSkill(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.isAutoModeCanUseSkill ? NodeStatus.Success : NodeStatus.Failure; 
    }
}
