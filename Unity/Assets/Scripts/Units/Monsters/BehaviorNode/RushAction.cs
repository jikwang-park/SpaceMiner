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
        if (context.TargetDistance > context.stats.range)
        {
            context.transform.Translate(Vector3.forward * context.stats.moveSpeed * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
