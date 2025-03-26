using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeUnitDamage : MonoBehaviour,IAttackable
{
    private CharacterStats stats;
    private bool gameObjectEnabled;
    private Unit unit;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        unit =  GetComponent<Unit>(); 
    }

    private void OnEnable()
    {
        gameObjectEnabled = true;
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        if (unit.HasBarrier)
        {
            unit.barrier -= attack.damage;
            if (unit.barrier < 0 )
            {
                unit.currentHp += unit.barrier;
                unit.barrier = 0;
            }
            
        }
        else
        {
            unit.currentHp -= attack.damage;
        }

        if (unit.currentHp < 0 && gameObjectEnabled)
        {
            unit.currentHp = new BigNumber("0");
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
