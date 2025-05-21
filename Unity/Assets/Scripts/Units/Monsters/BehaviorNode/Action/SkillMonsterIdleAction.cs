using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMonsterIdleAction : ActionNode<MonsterController>
{
    private MonsterSkill skill;

    public SkillMonsterIdleAction(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        if(context.status == MonsterController.Status.Dead)
        {
            return;
        }

        context.AnimationController.Play(AnimationControl.AnimationClipID.BattleIdle);
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.status == MonsterController.Status.Dead
            || context.CanAttack
            || (context.hasTarget && context.CanMove)
            || (skill.IsCoolTime && skill.IsTargetExist))
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Running;
    }
}
