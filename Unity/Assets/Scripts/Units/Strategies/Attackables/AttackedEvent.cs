using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedEvent : MonoBehaviour, IAttackable
{
    public event System.Action<AttackedEvent> OnAttacked;

    public void OnAttack(GameObject attacker, Attack attack)
    {
        OnAttacked?.Invoke(this);
    }
}
