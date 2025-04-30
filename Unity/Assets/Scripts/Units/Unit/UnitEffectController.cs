using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffectController : MonoBehaviour, IDestructable
{
    private PoolableEffect barrierEffect;
    private UnitStats unitStat;
    private AttackedTakeUnitDamage takeUnitDamage;
    public Transform attackEffectPoint;
    private void Awake()
    {
        unitStat = GetComponent<UnitStats>();
        takeUnitDamage = GetComponent<AttackedTakeUnitDamage>();
    }

    public void OnDestruction(GameObject Attacker)
    {
        DoBarrierDown();
        ParticleEffectManager.Instance.PlayOneShot("UnitDeathEffect", gameObject.transform.position);
        unitStat.OnAttack -= DoAttack;
        unitStat.OnBarrierUp -= DoBarrierUp;
        takeUnitDamage.OnBarrierDown -= DoBarrierDown;
    }

    private void OnEnable()
    {
        ParticleEffectManager.Instance.PlayOneShot("UnitChangeEffect", gameObject.transform);
        unitStat.OnAttack += DoAttack;
        unitStat.OnBarrierUp += DoBarrierUp;
        takeUnitDamage.OnBarrierDown += DoBarrierDown;

    }

    private void OnDisable()
    {
        unitStat.OnBarrierUp -= DoBarrierUp;
        takeUnitDamage.OnBarrierDown -= DoBarrierDown;
    }

    private void DoBarrierUp()
    {
        if(barrierEffect != null)
        {
            return;
        }
        barrierEffect = ParticleEffectManager.Instance.PlayBuffEffect("BarrierEffect", gameObject.transform);
    }
    private void DoBarrierDown()
    {
        if (barrierEffect != null)
        {
            ParticleEffectManager.Instance.StopBuffEffect(barrierEffect);
            barrierEffect = null;
        }
    }
    private void DoAttack(UnitTypes type)
    {
        if(attackEffectPoint != null)
        {
            ParticleEffectManager.Instance.PlayOneShot(type.ToString() +"AttackEffect", attackEffectPoint);
        }
    }
}
