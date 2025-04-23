using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public BigNumber maxHp = 0;

    public BigNumber damage;
    public BigNumber armor = 0;

    public float coolDown;
    public float range;
    public float criticalChance;
    public float criticalMultiplier;

    public float moveSpeed;

    public float HPRate { get; protected set; }

    [SerializeField]
    private BigNumber hp;

    public virtual BigNumber Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (maxHp != 0)
            {
                HPRate = hp.DivideToFloat(maxHp);
                OnHpChanged?.Invoke(HPRate);
            }
        }
    }

    public event System.Action<float> OnHpChanged;

    protected virtual void OnEnable()
    {
        Hp = maxHp;
    }

    protected virtual void OnDisable()
    {
        OnHpChanged = null;
    }

    public abstract Attack CreateAttack(CharacterStats defenderStats);
    //{
    //    //TODO: 대미지 계산식 정해지면 수정해야함 - 250322 HKY

    //    Attack attack = new Attack();

    //    BigNumber damage = attackerStats.damage;

    //    damage += this.damage;
    //    attack.isCritical = criticalChance >= Random.value;
    //    if (attack.isCritical)
    //    {
    //        damage *= criticalMultiplier;
    //    }
    //    attack.damage = damage;

    //    if (defenderStats != null)
    //    {
    //        attack.damage -= defenderStats.armor;
    //    }

    //    return attack;
    //}

    public abstract void Execute(GameObject defender);
}
