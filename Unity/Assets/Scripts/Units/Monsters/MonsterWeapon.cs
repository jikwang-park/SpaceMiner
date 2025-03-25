using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWeapon.asset", menuName = "Attack/MonsterWeapon")]
public class MonsterWeapon : AttackDefinition
{
    public override void Execute(GameObject attacker, GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot((attacker.transform.position - defender.transform.position), Vector3.forward);

        if (distance > range)
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
