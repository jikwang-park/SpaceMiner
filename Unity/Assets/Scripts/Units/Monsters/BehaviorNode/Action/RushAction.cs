using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAction : ActionNode<MonsterController>
{
    public RushAction(MonsterController context) : base(context)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();

        if (context.status == MonsterController.Status.Run
            || context.status == MonsterController.Status.Dead)
        {
            return;
        }
        context.status = MonsterController.Status.Run;
        context.AnimationController.Play(AnimationControl.AnimationClipID.Run);
    }

    protected override NodeStatus OnUpdate()
    {
        // TODO: 이후 이동방식 수정 혹은 검토 필요 - 250323 HKY
        if (context.status != MonsterController.Status.Dead
             && context.TargetDistance > context.Stats.range)
        {
            context.transform.Translate(Vector3.forward * context.Stats.moveSpeed * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
