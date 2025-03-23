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
        if (context.TargetDistance > context.weapon.range)
        {
            context.transform.Translate(Vector3.forward * context.Speed * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
