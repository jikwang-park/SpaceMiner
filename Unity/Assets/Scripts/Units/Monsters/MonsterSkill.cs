using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private MonsterSkillTable.Data skillData;
    private MonsterController controller;
    private float lastSkillTime;
    private MonsterStats stats;

    public bool IsCoolTime => lastSkillTime + skillData.CoolTime < Time.time;

    public Transform Target { get; private set; }

    [SerializeField]
    private float skillTime = 0.5f;

    public bool IsTargetExist
    {
        get
        {
            switch (skillData.TargetPriority)
            {
                case TargetPriority.FrontOrder:
                    for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
                    {
                        Target = controller.StageManager.UnitPartyManager.GetUnit((UnitTypes)i);
                        if (Target is not null)
                        {
                            return true;
                        }
                    }
                    break;
                case TargetPriority.BackOrder:
                    for (int i = (int)UnitTypes.Healer; i >= (int)UnitTypes.Tanker; --i)
                    {
                        Target = controller.StageManager.UnitPartyManager.GetUnit((UnitTypes)i);
                        if (Target is not null)
                        {
                            return true;
                        }
                    }
                    break;
            }
            Target = null;
            return false;
        }
    }


    private void Awake()
    {
        controller = GetComponent<MonsterController>();
    }

    private void Start()
    {
        if (controller.AnimationController.ContainsClip(AnimationControl.AnimationClipID.Skill))
        {
            controller.AnimationController.AddEvent(AnimationControl.AnimationClipID.Skill, skillTime, Execute);
            controller.AnimationController.AddEvent(AnimationControl.AnimationClipID.Skill, 1f, OnSkillEnd);
        }
        else
        {
            controller.AnimationController.AddEvent(AnimationControl.AnimationClipID.Attack, skillTime, Execute);
            controller.AnimationController.AddEvent(AnimationControl.AnimationClipID.Attack, 1f, OnSkillEnd);
        }
    }

    private void OnEnable()
    {
        lastSkillTime = Time.time;
    }

    public void SetSkill(int skillId, MonsterStats stats)
    {
        var skillData = DataTableManager.MonsterSkillTable.GetData(skillId);
        SetSkill(skillData, stats);
    }

    public void SetSkill(MonsterSkillTable.Data skillData, MonsterStats stats)
    {
        this.skillData = skillData;
        this.stats = stats;
    }

    public void Use()
    {
        controller.status = MonsterController.Status.SkillUsing;
        lastSkillTime = Time.time;
        if (controller.AnimationController.ContainsClip(AnimationControl.AnimationClipID.Skill))
        {
            controller.AnimationController.Play(AnimationControl.AnimationClipID.Skill);
        }
        else
        {
            controller.AnimationController.Play(AnimationControl.AnimationClipID.Attack);
        }
    }

    private void OnSkillEnd()
    {
        if(controller.status != MonsterController.Status.SkillUsing)
        {
            return;
        }

        lastSkillTime = Time.time;
        controller.AnimationController.Play(AnimationControl.AnimationClipID.BattleIdle);
        controller.status = MonsterController.Status.Wait;
    }

    public void Execute()
    {
        if (controller.status != MonsterController.Status.SkillUsing)
        {
            return;
        }

        List<Transform> targets = new List<Transform>();

        switch (skillData.TargetPriority)
        {
            case TargetPriority.FrontOrder:
                for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
                {
                    Transform target = controller.StageManager.UnitPartyManager.GetUnit((UnitTypes)i);
                    if (target is null)
                    {
                        continue;
                    }

                    var tDistance = transform.position.z - target.position.z;
                    if (tDistance > skillData.SkillRange)
                    {
                        continue;
                    }

                    targets.Add(target);

                    if (targets.Count >= skillData.MaxTargetCount)
                    {
                        break;
                    }
                }
                break;
            case TargetPriority.BackOrder:
                for (int i = (int)UnitTypes.Healer; i >= (int)UnitTypes.Tanker; --i)
                {
                    Transform target = controller.StageManager.UnitPartyManager.GetUnit((UnitTypes)i);
                    if (target is null)
                    {
                        continue;
                    }

                    var tDistance = transform.position.z - target.position.z;
                    if (tDistance > skillData.SkillRange)
                    {
                        continue;
                    }

                    targets.Add(target);

                    if (targets.Count >= skillData.MaxTargetCount)
                    {
                        break;
                    }
                }
                break;
        }

        foreach (var defender in targets)
        {
            CharacterStats dStats = defender.GetComponent<CharacterStats>();
            Attack attack = stats.CreateAttack(dStats, skillData.AttackRatio);
            IAttackable[] attackables = defender.GetComponents<IAttackable>();
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack);
            }
        }
    }
}
