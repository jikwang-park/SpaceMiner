using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public BigNumber maxHp;
    public BigNumber damage;
    public BigNumber armor = 0;

    public float coolDown;
    public float range;
    public float criticalChance;
    public float criticalMultiplier;

    public float moveSpeed;

    public BigNumber Hp { get; set; }

    protected virtual void OnEnable()
    {
        Hp = maxHp;
    }

    public abstract Attack CreateAttack(CharacterStats defenderStats);
    //{
    //    //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY

    //    Attack attack = new Attack();

    //    BigNumber damage = attackerStats.damage;

    //    damage += this.damage;
    //    attack.isCritical = criticalChance >= Random.value;
    //    if (attack.isCritical)
    //    {
    //        damage *= criticalMultiplier;
    //    }
    //    attack.damage = damage;

    //    if (defenderStats != null)
    //    {
    //        attack.damage -= defenderStats.armor;
    //    }

    //    return attack;
    //}

    public abstract void Execute(GameObject defender);
}
