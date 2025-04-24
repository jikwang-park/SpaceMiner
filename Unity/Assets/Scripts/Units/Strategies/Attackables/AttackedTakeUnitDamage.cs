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
        //TODO: 유닛 스탯으로 배리어 이동 후 작업 필요
        if(unit.unitStats.barrier > attack.damage)
        {
            unit.unitStats.barrier -= attack.damage;
            return;
        }
        else
        {
            var trueDamage = attack.damage - unit.unitStats.barrier;
            unit.unitStats.Hp -= trueDamage;
            unit.unitStats.barrier = 0;

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
        }

       
    }
}
