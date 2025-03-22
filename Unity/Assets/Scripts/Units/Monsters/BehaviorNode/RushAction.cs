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
        // TODO: 수정필요
        if (context.TargetDistance > context.weapon.range)
        {
            context.transform.Translate(Vector3.forward * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
