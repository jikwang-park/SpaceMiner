using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDefinition : ScriptableObject
{
    public AssetReferenceGameObject weaponPrefab;
    public float coolDown;
    public float range;
    public BigNumber damage;
    public float criticalChance;
    public float CriticalMultiplier;

    public Attack CreateAttack(CharacterStats attackerStats, CharacterStats defenderStats)
    {
        //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY

        Attack attack = new Attack();

        BigNumber damage = attackerStats.damage;

        damage += this.damage;
        attack.critical = criticalChance >= Random.value;
        if (attack.critical)
        {
            damage *= CriticalMultiplier;
        }
        attack.damage = damage;

        if (defenderStats != null)
        {
            attack.damage -= defenderStats.armor;
        }

        return attack;
    }

    public virtual void Execute(GameObject attacker, GameObject defender)
    {

    }
    public virtual void Execute(GameObject attacker, Collider[] defender, int count)
    {

    }
}
