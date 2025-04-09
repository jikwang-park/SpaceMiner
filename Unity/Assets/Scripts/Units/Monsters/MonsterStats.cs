using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : CharacterStats
{
    public void SetData(MonsterTable.Data monsterData)
    {
        damage = monsterData.Atk;
        range = monsterData.AtkRange;
        maxHp = monsterData.Hp;
        Hp = maxHp;
        coolDown = 100f / monsterData.AtkSpeed;
        moveSpeed = monsterData.MoveSpeed;
    }

    public override void Execute(GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = transform.position.z - defender.transform.position.z;

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }

    public override Attack CreateAttack(CharacterStats defenderStats)
    {
        //���� ���ݷ� * 200/(200 + ����)

        Attack attack = new Attack();

        BigNumber damage = this.damage;

        attack.isCritical = criticalChance >= Random.value;
        if (attack.isCritical)
        {
            damage *= criticalMultiplier;
        }
        attack.damage = damage;

        if (defenderStats is not null)
        {
            attack.damage *= Variables.DefenceBase.DivideToFloat(200 + defenderStats.armor);
        }

        return attack;
    }

    public Attack CreateAttack(CharacterStats defenderStats, float skillRatio)
    {
        //���� ���ݷ� * ��ų ���� * 200/(200 + ����)

        Attack attack = new Attack();

        BigNumber damage = this.damage;

        attack.isCritical = criticalChance >= Random.value;
        if (attack.isCritical)
        {
            damage *= criticalMultiplier;
        }
        attack.damage = damage;

        if (defenderStats is not null)
        {
            attack.damage *= skillRatio * Variables.DefenceBase.DivideToFloat(200 + defenderStats.armor);
        }

        return attack;
    }
}