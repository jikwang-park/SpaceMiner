using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    private MonsterSkillTable.Data skillData;
    private MonsterController controller;
    private float lastSkillTime;

    public bool IsCoolTime => lastSkillTime + skillData.CoolTime < Time.time;

    public Transform Target { get; private set; }

    public bool IsTargetExist
    {
        get
        {
            switch (skillData.Type)
            {
                case MonsterSkillTable.TargetPriority.FrontOrder:
                    for (int i = (int)UnitTypes.Tanker; i <= (int)UnitTypes.Healer; ++i)
                    {
                        Target = controller.StageManager.UnitPartyManager.GetUnit((UnitTypes)i);
                        if (Target is not null)
                        {
                            return true;
                        }
                    }
                    break;
                case MonsterSkillTable.TargetPriority.BackOrder:
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

    public void SetSkill(int skillId)
    {
        skillData = DataTableManager.MonsterSkillTable.GetData(skillId);
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
            case MonsterSkillTable.TargetPriority.FrontOrder:
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
            case MonsterSkillTable.TargetPriority.BackOrder:
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
            Attack attack = CreateAttack(dStats);
            IAttackable[] attackables = defender.GetComponents<IAttackable>();
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack);
            }
        }
    }

    public Attack CreateAttack(CharacterStats defenderStats)
    {
        //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY
        Attack attack = new Attack();

        attack.damage = skillData.AtkRatio;
        return attack;
    }
}
