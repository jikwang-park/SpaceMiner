using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitStats : CharacterStats
{
    BigNumber specialPoint;

    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = data.MoveSpeed;
        maxHp = (int)data.Basic_HP;
        Hp = maxHp;

        coolDown = 1;
        armor = (int)data.Basic_DP;
        damage = (int)data.Basic_AP;
        range = (int)data.Distance;

        switch (type)
        {
            case UnitTypes.Tanker:
                specialPoint = (int)data.Special_DR;
                break;
            case UnitTypes.Dealer:
                specialPoint = (int)data.Special_CD;
                break;
            case UnitTypes.Healer:
                specialPoint = (int)data.Special_H;
                break;
        }
    }


    public override void Execute(GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

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
        //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY
        Attack attack = new Attack();

        BigNumber damage = this.damage;

        attack.isCritical = criticalChance >= Random.value;
        if (attack.isCritical)
        {
            damage *= criticalMultiplier;
        }
        attack.damage = damage;

        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }
}
