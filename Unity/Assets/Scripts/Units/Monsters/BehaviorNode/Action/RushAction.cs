using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAction : ActionNode<MonsterController>
{
    public RushAction(MonsterController context) : base(context)
    {
    }

    protected override NodeStatus OnUpdate()
    {
        // TODO: ���� �̵���� ���� Ȥ�� ���� �ʿ� - 250323 HKY
        if (context.TargetDistance > context.Stats.range)
        {
            context.transform.Translate(Vector3.forward * context.Stats.moveSpeed * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
