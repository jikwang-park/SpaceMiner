using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTakeUnitDamage : MonoBehaviour,IAttackable
{
    private CharacterStats stats;
    private bool gameObjectEnabled;
    private Unit unit;

    public event Action<float> OnHpChanged;

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
            unit.unitStats.barrier -= attack.damage;
            if (unit.unitStats.barrier < 0 )
            {
                unit.unitStats.Hp += unit.unitStats.barrier;
                unit.unitStats.barrier = 0;
            }
        }
        else
        {
            unit.unitStats.Hp -= attack.damage;
        }

        if (unit.unitStats.Hp < 0 && gameObjectEnabled)
        {
            unit.unitStats.Hp = new BigNumber("0");
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
