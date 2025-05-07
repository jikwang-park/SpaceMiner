using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveOnDeath : MonoBehaviour
{
    public bool HasRevived { get; private set; } = false;
    private float reviveHpPercent;

    public void Initialize(float hpPercent)
    {
        reviveHpPercent = hpPercent;
        HasRevived = false;
    }

    public void DoRevive()
    {
        if (HasRevived) return;

        HasRevived = true;

        var unit = GetComponent<Unit>();
        BigNumber revivedHp = (unit.unitStats.maxHp * reviveHpPercent) / 100f;
        unit.unitStats.Hp = revivedHp;

    }
}
