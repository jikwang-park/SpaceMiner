using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private CharacterStats stats;
    private bool gameObjectEnabled;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        gameObjectEnabled = true;
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        stats.Hp -= attack.damage;

        if (stats.Hp < 0 && gameObjectEnabled)
        {
            stats.Hp = new BigNumber("0");
            IDestructable[] destructables = GetComponents<IDestructable>();
            if (destructables.Length > 0)
            {
                gameObjectEnabled = false;
            }
            foreach (var destructable in destructables)
            {
                destructable.OnDestruction(attacker);
            }
        }
    }
}
