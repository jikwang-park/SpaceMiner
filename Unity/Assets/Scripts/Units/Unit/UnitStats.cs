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
                criticalChance += (amount*100);
                break;
            case UpgradeType.CriticalDamages:
                criticalMultiplier += (amount * 100);
                break;
        }
    }

    public void AddBuildingStats(BuildingTable.BuildingType type , float amount)
    {
        if(amount == 0)
        {
            return;
        }

        switch (type)
        {
            case BuildingTable.BuildingType.IdleTime:
                break;
            case BuildingTable.BuildingType.AttackPoint:
                damage *= (int)amount;
                break;
            case BuildingTable.BuildingType.HealthPoint:
                maxHp *= (int)amount;
                break;
            case BuildingTable.BuildingType.DefensePoint:
                armor *= (int)amount;
                break;
            case BuildingTable.BuildingType.CriticalPossibility:
                criticalChance += amount;
                break;
            case BuildingTable.BuildingType.CriticalDamages:
                criticalMultiplier += (int)amount;
                break;
            case BuildingTable.BuildingType.Gold:
                break;
            case BuildingTable.BuildingType.Mining:
                break;
        }
    }



    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = 5f;
        maxHp = 1000;/*(int)data.Basic_HP;*/
        Hp = maxHp;

        coolDown = 1;
        armor = (int)data.Defence;
        damage = (int)data.Attack;
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
    public void SkillExecute(GameObject defender) // 스킬 인포및 디펜더 정보 넘겨서 데미지 처리
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
        //나중에 추가 해야됌
        Attack attack = new Attack();

        var dealerData = DataTableManager.DealerSkillTable.GetData(1101); //250331 HKY 데이터형 변경

        BigNumber damage = this.damage;

        attack.isCritical = criticalChance >= Random.Range(0,100);
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
        //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY
        Attack attack = new Attack();

        BigNumber damage = this.damage;


        attack.isCritical = criticalChance >= Random.Range(0, 100);
        if (attack.isCritical)
        {
            damage = (damage * 2) + (damage * criticalMultiplier);
            Debug.Log(damage);
            Debug.Log(criticalChance);

        }
        attack.damage = damage;

        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }
}
