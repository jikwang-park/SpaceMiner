using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class IsTankerCanUseSkillCondition : ConditionNode<Unit>
{
    public IsTankerCanUseSkillCondition(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsTankerCanUseSkill ? NodeStatus.Success : NodeStatus.Failure;
    }
}
