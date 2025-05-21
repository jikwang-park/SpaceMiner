using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnitUpgradeTable;

public class UnitStats : CharacterStats
{



    public void AddStats(UpgradeType type , float amount)
    {
        switch (type)
        {
            case UpgradeType.AttackPoint:
                damage += (int)amount;
                break;
            case UpgradeType.HealthPoint:
                maxHp += (int)amount;
                break;
            case UpgradeType.DefensePoint:
                armor += (int)amount;
                break;
            case UpgradeType.CriticalPossibility:
                //���߿� �ٲ����
                break;
            case UpgradeType.CriticalDamages:
                break;
        }
    }


    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = 5f;
        maxHp = 1000;/*(int)data.Basic_HP;*/
        Hp = maxHp;

        coolDown = 1;
        armor = (int)data.Basic_DP;
        damage = (int)data.Basic_AP;
        //range = (int)data.Distance;

        switch (type)
        {
            case UnitTypes.Tanker:
                range = 1f;
                break;
            case UnitTypes.Dealer:
                range = 5.5f;
                break;
            case UnitTypes.Healer:
                range = 10f;
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

        var dealerData = DataTableManager.DealerSkillTable.GetData(1101); //250331 HKY �������� ����

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
