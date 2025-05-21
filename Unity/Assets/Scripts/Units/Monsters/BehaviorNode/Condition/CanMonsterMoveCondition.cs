using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMonsterMoveCondition : ConditionNode<MonsterController>
{
    public CanMonsterMoveCondition(MonsterController context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.CanMove ? NodeStatus.Success : NodeStatus.Failure;
    }
}
