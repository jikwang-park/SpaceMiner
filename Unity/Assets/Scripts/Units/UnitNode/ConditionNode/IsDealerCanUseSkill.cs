using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class IsDealerCanUseSkill : ConditionNode<Unit>
{
    public IsDealerCanUseSkill(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.IsDealerCanUseSkill ? NodeStatus.Success : NodeStatus.Failure;
    }
}
