using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMonsterAction : ActionNode<MonsterController>
{
    private MonsterSkill skill;

    public SkillMonsterAction(MonsterController context) : base(context)
    {
        skill = context.GetComponent<MonsterSkill>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        Debug.Log("Skill");
        skill.Use();
    }

    protected override NodeStatus OnUpdate()
    {
        if (context.status == MonsterController.Status.SkillUsing)
        {
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
    }
}
