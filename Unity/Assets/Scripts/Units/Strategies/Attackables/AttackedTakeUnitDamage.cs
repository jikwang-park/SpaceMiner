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
    public event Action<GameObject> GetDamaged;
    public event Action<Unit> OnDamageOverflowed;
    public event Action OnBarrierDown;


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
        

        GetDamaged?.Invoke(attacker);

        if (unit.unitStats.barrier > attack.damage)
        {
            unit.unitStats.barrier -= attack.damage;
            
            return;
        }
        else
        {
            if(unit.unitStats.barrier > 0)
            {
                OnBarrierDown?.Invoke();
            }
            var trueDamage = attack.damage - unit.unitStats.barrier;
            unit.unitStats.Hp -= trueDamage;
            unit.unitStats.barrier = 0;
            //if(unit.unitStats.Hp <= 0)
            //{
            //    OnDamageOverflowed?.Invoke(unit);
            //    return;
            //}

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
