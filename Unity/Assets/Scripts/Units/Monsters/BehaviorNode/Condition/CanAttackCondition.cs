using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttackMonsterCondition : ConditionNode<MonsterController>
{
    public CanAttackMonsterCondition(MonsterController context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        return context.CanAttack ? NodeStatus.Success : NodeStatus.Failure;
    }
}
