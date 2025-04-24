using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitUpgradeTable;


public class UnitStats : CharacterStats
{
    public float attackSpeed { get; private set; } = 0;
    private StageManager stageManager;

    private Grade currentGrade;



    public BigNumber barrier=0;



    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void Start()
    {

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
       
        attackSpeed = data.AttackSpeed;
        this.type = type;

      
        currentGrade = data.Grade;

        coolDown = 1;
        range = data.Range;
        maxHp = UnitCombatPowerCalculator.statsDictionary[type].soldierMaxHp;

        RecalculateHpWithRatio();
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

        attack.isCritical = criticalChance >= Random.Range(0, 100);
        if (attack.isCritical)
        {
           var multiplier =  stats.criticalMultiplier;

            attack.damage = stats.soldierAttack * multiplier;
        }
        else
        {
            attack.damage = stats.soldierAttack;
        }

        return attack;
    }
    public void SkillExecute(GameObject defender) // ��ų ������ �����? ���� �Ѱܼ� ������ ó��
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

        var data = SaveLoadManager.Data.unitSkillUpgradeData.skillUpgradeId;

        var stats = UnitCombatPowerCalculator.statsDictionary[type];

        var id = data[UnitTypes.Dealer][currentGrade];

        var currentData = DataTableManager.DealerSkillTable.GetData(id);

        BigNumber finialSkillDamage = stats.soldierAttack * currentData.DamageRatio;


       var  criticalChance = stats.criticalPossibility;

        attack.isCritical = criticalChance >= Random.Range(0, 100);
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
    public void UseShiled(float duration, BigNumber amount)
    {
        barrier += amount;
        StartCoroutine(RemoveBarrierAfterDuration(duration, amount));
    }


    private IEnumerator RemoveBarrierAfterDuration(float duration, BigNumber amount)
    {
        yield return new WaitForSeconds(duration);
        barrier -= amount;

        if(barrier < 0)
            barrier = 0;
    }
   
}
