using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IsSkillButtonPressed : ConditionNode<Unit>
{
    public IsSkillButtonPressed(Unit context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.isSkillButtonPressed ? NodeStatus.Success : NodeStatus.Failure;
    }
}
