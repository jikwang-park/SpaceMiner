using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedDebug : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        if (attack.isCritical)
        {
            Debug.Log("크리티컬!");
        }
        Debug.Log($"{Time.time} {attacker.name} => {gameObject.name} : {attack.damage}");
    }
}
