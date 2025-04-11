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

    public bool IsTargetExist
    {
        get
        {
            switch (skillData.Type)
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


    private void OnEnable()
    {
        lastSkillTime = Time.time;
    }

    public void SetSkill(int skillId, MonsterStats stats)
    {
        skillData = DataTableManager.MonsterSkillTable.GetData(skillId);
        this.stats = stats;
    }

    public void Use()
    {
        controller.status = MonsterController.Status.SkillUsing;
        lastSkillTime = Time.time;
        StartCoroutine(CoSkill());
    }

    public IEnumerator CoSkill()
    {
        yield return new WaitForSeconds(0.25f);
        Execute();
        yield return new WaitForSeconds(0.25f);
        controller.status = MonsterController.Status.Wait;
    }

    public void Execute()
    {
        controller.status = MonsterController.Status.SkillUsing;

        List<Transform> targets = new List<Transform>();

        switch (skillData.Type)
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

                    if (targets.Count >= skillData.MaxCount)
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

                    if (targets.Count >= skillData.MaxCount)
                    {
                        break;
                    }
                }
                break;
        }

        foreach (var defender in targets)
        {
            CharacterStats dStats = defender.GetComponent<CharacterStats>();
            Attack attack = stats.CreateAttack(dStats, skillData.AtkRatio);
            IAttackable[] attackables = defender.GetComponents<IAttackable>();
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack);
            }
        }
    }
}
