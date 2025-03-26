using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class IsUnitCanUseSkillCondition : ConditionNode<Unit>
{
    public IsUnitCanUseSkillCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.CanUseSkill ? NodeStatus.Success : NodeStatus.Failure;
    }
}
