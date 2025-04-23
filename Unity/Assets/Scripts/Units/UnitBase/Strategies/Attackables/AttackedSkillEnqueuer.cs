using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedSkillEnqueuer : MonoBehaviour, IAttackable
{
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void OnAttack(GameObject attacker, Attack attack)
    {
        unit.EnqueueSkill();
    }
}
