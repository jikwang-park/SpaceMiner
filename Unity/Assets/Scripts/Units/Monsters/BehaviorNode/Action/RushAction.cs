using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        if (context.StageManager.IngameStatus == IngameStatus.Mine)
        {
            context.NavMeshAgent.enabled = true;
            context.NavMeshAgent.isStopped = false;
            context.NavMeshAgent.SetDestination(context.Target.position);
        }
    }

    protected override NodeStatus OnUpdate()
    {
        // TODO: 이후 이동방식 수정 혹은 검토 필요 - 250323 HKY
        if (context.StageManager.IngameStatus == IngameStatus.Mine)
        {
            if (context.TargetDistance > context.Stats.range)
            {
                return NodeStatus.Running;
            }
            else
            {
                context.NavMeshAgent.isStopped = true;
                context.NavMeshAgent.enabled = false;
            }
        }
        else if (context.status != MonsterController.Status.Dead
                  && context.TargetDistance > context.Stats.range)
        {
            context.transform.Translate(Vector3.forward * context.Stats.moveSpeed * Time.deltaTime);
        }

        return NodeStatus.Success;
    }
}
