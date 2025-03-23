using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private CharacterStats stats;


    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        stats.Hp -= attack.damage;

        if (stats.Hp < 0)
        {
            stats.Hp = new BigNumber("0");
            IDestructable[] destructables = GetComponents<IDestructable>();
            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(attacker);
            }
        }
    }
}
