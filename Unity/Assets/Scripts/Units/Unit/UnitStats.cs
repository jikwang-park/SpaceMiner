using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitUpgradeTable;


public class UnitStats : CharacterStats
{
    private StageManager stageManager;

    private Grade currentGrade;

    public BigNumber barrier = 0;

    public bool hasBarrier = false;

    private BigNumber buffReflectionDamage;

    private System.Action<GameObject> ReflectDelegate;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UnitCombatPowerCalculator.onCombatPowerChanged += RefreshStats;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnitCombatPowerCalculator.onCombatPowerChanged -= RefreshStats;
    }
    private void RefreshStats()
    {
        float prevRate = maxHp != 0 ? Hp.DivideToFloat(maxHp) : 1f;

        armor = UnitCombatPowerCalculator.statsDictionary[type].soldierDefense;
        maxHp = UnitCombatPowerCalculator.statsDictionary[type].soldierMaxHp;
        range = UnitCombatPowerCalculator.statsDictionary[type].attackRange;
        coolDown = UnitCombatPowerCalculator.statsDictionary[type].coolDown;
        moveSpeed = UnitCombatPowerCalculator.statsDictionary[type].moveSpeed;

        Hp = maxHp * prevRate;
    }
    private void RecalculateHpWithIncrease()
    {

        BigNumber previousMaxHp = maxHp;

        maxHp = UnitCombatPowerCalculator.statsDictionary[type].soldierMaxHp;

        BigNumber increase = maxHp - previousMaxHp;

        Hp += increase;
        if (Hp > maxHp)
            Hp = maxHp;
    }

    private void RecalculateHpWithRatio()
    {

        float ratio = 0f;
        if (maxHp > 0)
        {
            ratio = Hp.DivideToFloat(maxHp);
        }
        else
        {
            ratio = 1f;
        }
        maxHp = UnitCombatPowerCalculator.statsDictionary[type].soldierMaxHp;
        Hp = maxHp * ratio;
       
    }


    public UnitTypes type;
    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        moveSpeed = data.MoveSpeed;

        coolDown = 100f / data.AttackSpeed;
        this.type = type;
        

        currentGrade = data.Grade;

        coolDown = 1;
        range = data.Range;
        armor = UnitCombatPowerCalculator.statsDictionary[type].soldierDefense;
        maxHp = UnitCombatPowerCalculator.statsDictionary[type].soldierMaxHp;
        range = UnitCombatPowerCalculator.statsDictionary[type].attackRange;
        coolDown = UnitCombatPowerCalculator.statsDictionary[type].coolDown;
        moveSpeed = UnitCombatPowerCalculator.statsDictionary[type].moveSpeed;

        if(Hp == null)
        {
            Hp = maxHp;
        }
        else
        {
            RecalculateHpWithRatio();
        }

    }

    private void Update()
    {
        Debug.Log(Hp);

    }
    public override void Execute(GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }
    public override Attack CreateAttack(CharacterStats defenderStats)
    {
        Attack attack = new Attack();

        var stats = UnitCombatPowerCalculator.statsDictionary[type];
        var criticalChance = stats.criticalPossibility;

        attack.isCritical = criticalChance >= Random.Range(0f, 1f);
        if (attack.isCritical)
        {
            var multiplier = stats.criticalMultiplier;

            attack.damage = stats.soldierAttack * multiplier;
        }
        else
        {
            attack.damage = stats.soldierAttack;
        }

        return attack;
    }
    public void SkillExecute(GameObject defender)
    {
        if (defender is null)
        {
            return;
        }
        var distance = Vector3.Dot(transform.position - defender.transform.position, Vector3.forward);

        if (distance > range)
        {
            return;
        }

        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateDealerSkillAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }

    public Attack CreateDealerSkillAttack(CharacterStats defenderStats)
    {
        Attack attack = new Attack();

        var stats = UnitCombatPowerCalculator.statsDictionary[type];

        var unit = GetComponent<Unit>();

        BigNumber finialSkillDamage = stats.soldierAttack * unit.Skill.Ratio;


        var criticalChance = stats.criticalPossibility;

        attack.isCritical = criticalChance >= Random.Range(0f, 1f);
        if (attack.isCritical)
        {
            var multiplier = stats.criticalMultiplier;
            attack.damage = finialSkillDamage * multiplier;
        }
        else
        {
            attack.damage = finialSkillDamage;
        }


        //if (defenderStats != null)
        //{
        //    attack.damage -= defenderStats.armor;
        //}
    
        return attack;
    }
    public void UseTankerBuff(float duration , BigNumber amount)
    {
        buffReflectionDamage = amount;
        var takeDamage = GetComponent<AttackedTakeUnitDamage>();
        ReflectDelegate = GetReflectionDamage;

        takeDamage.GetDamaged += ReflectDelegate;

        StartCoroutine(RemoveBuffAfterDuration(duration,takeDamage));
    }

    public void GetReflectionDamage(GameObject defender)
    {
        
        CharacterStats dStats = defender.GetComponent<CharacterStats>();
        Attack attack = CreateBuffAttack(dStats);
        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(gameObject, attack);
        }
    }

    private Attack CreateBuffAttack(CharacterStats defenderStats)
    {
        Attack attack = new Attack();
        attack.damage = buffReflectionDamage;
        return attack;
    }

    private IEnumerator RemoveBuffAfterDuration(float duration , AttackedTakeUnitDamage attackedDamage)
    {
        yield return new WaitForSeconds(duration);
        if(ReflectDelegate != null)
        {
            attackedDamage.GetDamaged -= ReflectDelegate;
            ReflectDelegate = null;
        }
    }

    public void UseShiled(float duration, BigNumber amount)
    {
        hasBarrier = true;
        barrier += amount;
        StartCoroutine(RemoveBarrierAfterDuration(duration, amount));
    }


    private IEnumerator RemoveBarrierAfterDuration(float duration, BigNumber amount)
    {
        yield return new WaitForSeconds(duration);
        barrier -= amount;

        if (barrier < 0)
            barrier = 0;
        hasBarrier = false;
    }

    //public void UseHealerBuff(BigNumber hpAmount)
    //{
    //    var takeDamage = GetComponent<AttackedTakeUnitDamage>();
    //    takeDamage.OnDamageOverflowed += (unit) => unit.unitStats.Hp = hpAmount;
    //}



}
