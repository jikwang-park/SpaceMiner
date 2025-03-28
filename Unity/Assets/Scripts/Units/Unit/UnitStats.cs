using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitStats : CharacterStats
{
    BigNumber specialPoint;

    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = 5f;
        maxHp = 1000;/*(int)data.Basic_HP;*/
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
    public void SkillExecute(GameObject defender) // ��ų ������ ����� ���� �Ѱܼ� ������ ó��
    {
        if(defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateSkillAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
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

    public Attack CreateSkillAttack(CharacterStats defenderStats)
    {
        //���߿� �߰� �ؾ߉�
        Attack attack = new Attack();

        var dealerData = DataTableManager.DealerSkillTable.GetData("�븻������ųLv1");

        BigNumber damage = this.damage;

        attack.isCritical = criticalChance >= Random.value;
        if(attack.isCritical)
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

    public override Attack CreateAttack(CharacterStats defenderStats)
    {
        //TODO: ����� ���� �������� �����ؾ��� - 250322 HKY
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
