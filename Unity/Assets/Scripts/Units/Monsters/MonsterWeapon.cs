using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWeapon.asset", menuName = "Attack/MonsterWeapon")]
public class MonsterWeapon : AttackDefinition
{
    public override void Execute(GameObject attacker, GameObject defender)
    {
        if (defender == null)
        {
            return;
        }
        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        if (distance > range)
        {
            return;
        }

        Vector3 toTarget = (defender.transform.position - attacker.transform.position).normalized;
        float dot = Vector3.Dot(attacker.transform.forward, toTarget);
        if (dot < 0.5f)
        {
            return;
        }

        CharacterStats aStats = attacker.GetComponent<CharacterStats>();
        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateAttack(aStats, dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }
}
