using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;




public class Unit : MonoBehaviour
{
    public enum UnitStatus
    {
        Attacking,
        UsingSkill,
        Wait,
    }

    public UnitStatus currentStatus;


    //Base Stats\
    public UnitTypes UnitTypes
    {
        get
        {
            return currentUnitType;
        }

    }
    private UnitTypes currentUnitType;

    public BigNumber barrier;
    public bool HasBarrier
    {
        get
        {
            if (barrier > 0)
                return true;

            return false;
        }
    }

    public StageManager stageManger;

    public UnitStats unitStats;
    public bool IsInAttackRange
    {
        get
        {
           if(targetDistance <= unitStats.range)
                return true;

           return false;
        }
    }
    public float targetDistance;

    public BehaviorTree<Unit> behaviorTree;

    public float attackUsingTime = 0.4f;

    public int unitAliveCount = 0;

    [SerializeField]
    public UnitSkill unitSkill;

    public Transform targetPos;

    private bool isTargetInArea = false;

    private int lane = 0;

    private int currentUnitNum = 0;


    public float lastSkillUsedTime;


    public bool isAutoSkillMode;
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        stageManger = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        isAutoSkillMode = true;
    }

    public bool IsDead 
    {
        get
        {
            if (unitStats.Hp <= 0)
            {
                currentUnitNum++;
                return true;
            }
            return false;
                       }
    }
    
    public float RemainSkillCoolTime
    {
        get
        {
            switch (UnitTypes)
            {
                case UnitTypes.Tanker:
                    return (Time.time - lastSkillUsedTime) / unitSkill.coolTime;

                case UnitTypes.Dealer:
                    return (Time.time - lastSkillUsedTime) / unitSkill.coolTime;

                case UnitTypes.Healer:
                    return (Time.time - lastSkillUsedTime) / unitSkill.coolTime;
                default:
                    return 0;
            }
        }
    }



 

    public bool IsTankerCanUseSkill
    {
        get
        {
            
            if (currentUnitType == UnitTypes.Tanker)
            {
                if (Time.time < unitSkill.coolTime + lastSkillUsedTime)
                    return false;

                return true;
            }
            return false;
        }
    }

    public bool IsDealerCanUseSkill
    {
        get
        {
            if (currentUnitType == UnitTypes.Dealer)
            {
                if (targetDistance > unitStats.range ||
                        Time.time < unitSkill.coolTime + lastSkillUsedTime)
                    return false;
                
                return true;
            }
            return false;
        }
    }

    public bool IsHealerCanUseSkill
    {
        get
        {
            if (currentUnitType == UnitTypes.Healer)
            {
                if (unitSkill.targetList == null ||
                    Time.time < unitSkill.coolTime + lastSkillUsedTime)
                    return false;
                
                return true;
            }
            return false;
        }
    }
    public bool IsUnitCanAttack 
    {
        get
        {
            if (targetPos == null)
                return false;

            if (targetDistance <= unitStats.range && IsAttackCoolTimeOn)
            {
                return true;
            }
            return false;
        }
    }
    public bool IsNormalAttacking;

    public bool IsUnitHit;
    public bool IsSkillCoolTimeOn
    {
        get
        {
            if (Time.time > lastSkillUsedTime + unitSkill.coolTime)
                return true;

            return false;
        }
    }
    public bool IsAttackCoolTimeOn
    {
        get
        {
            if (Time.time > lastAttackTime + unitStats.coolDown)
                return true;

            return false;
        }
    }

    



    public float lastAttackTime;
    

    private bool isMonsterSpawn;


    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        unitStats.SetData(data, type);

        currentUnitType = type;
        switch (currentUnitType)
        {
            case UnitTypes.Tanker:
                unitSkill = gameObject.AddComponent<TankerSkill>();
                break;
            case UnitTypes.Dealer:
                unitSkill = gameObject.AddComponent<DealerSkill>();
                break;
            case UnitTypes.Healer:
                unitSkill = gameObject.AddComponent<HealerSkill>();
                break;
        }
        behaviorTree = UnitBTManager.SetBehaviorTree(this, currentUnitType);
    }
    public bool IsMonsterExist()
    {
        var lane = stageManger.StageMonsterManager.LaneCount;

        for (int i = 0; i < lane; ++i)
        {
            var target = stageManger.StageMonsterManager.GetMonsterCount(i);

            if (target > 0)
            {
                return true;

            }
        }
        return false;
    }
    private Transform GetTargetPosition()
    {
        var lane = stageManger.StageMonsterManager.LaneCount;

        for (int i = 0; i < lane; ++i)
        {
            var target = stageManger.StageMonsterManager.GetMonsterCount(i);
            var targetPosition = stageManger.StageMonsterManager.GetFirstMonster(i);

            if (target > 0)
            {
                var units = stageManger.UnitPartyManager.generateInstance;

                for(int j=0; j<units.Count; ++j)
                {
                    var unit = units[j];

                    targetDistance = Vector3.Dot(targetPosition.position - unit.transform.position, Vector3.forward);
                    if (targetDistance <= unitStats.range)
                    {
                        targetPos = targetPosition;
                        targetPos.gameObject.GetComponent<DestructedDestroyEvent>().OnDestroyed += (_) => targetPos = null;
                        return targetPos;
                    }
                }

          
            }
        }
        return null;
    }
    public void Move()
    {
        transform.position += Vector3.forward * Time.deltaTime * unitStats.moveSpeed;
    }

    public void AttackCorutine()
    {
        currentStatus = UnitStatus.Attacking;
        StartCoroutine(NormalAttackCor());
        lastAttackTime = Time.time;
    }
    public void UseSkill()
    {
        
        currentStatus = UnitStatus.UsingSkill;

        switch (currentUnitType)
        {
            case UnitTypes.Tanker:
                StartCoroutine(TankerSkillTimer());
                lastSkillUsedTime = Time.time;
                break;
            case UnitTypes.Dealer:
                StartCoroutine(DealerSkillTimer());
                lastSkillUsedTime = Time.time;
                break;
            case UnitTypes.Healer:
                StartCoroutine(HealerSkillTimer());
                lastSkillUsedTime = Time.time;
                break;
        }
    }

    public IEnumerator NormalAttackCor()
    {
        if (targetPos.gameObject != null)
        {
            unitStats.Execute(targetPos.gameObject);
        }
        yield return new WaitForSeconds(attackUsingTime);
        currentStatus = UnitStatus.Wait;
    }

    private IEnumerator HealerSkillTimer()
    {
        unitSkill.GetTarget();
        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(2.5f);
        lastAttackTime = Time.time;
        currentStatus = UnitStatus.Wait;
    }


    private IEnumerator DealerSkillTimer()
    {

        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(2f);
        currentStatus = UnitStatus.Wait;
    }
    private IEnumerator TankerSkillTimer()
    {
        unitSkill.GetTarget();
        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(2f);
        currentStatus = UnitStatus.Wait;
    }

    private float skillEndTime;

    public void SetBarrier(float time, BigNumber amount)
    {
        skillEndTime = Time.time + time;
        barrier = amount;

    }

    private void Update()
    {
        if (HasBarrier)
        {
            if (Time.time > skillEndTime)
            {
                barrier = 0;
            }
        }

        GetTargetPosition();
        behaviorTree.Update();

        IsUnitHit = false;
        if (Input.GetKeyDown(KeyCode.M))
        {
            IsUnitHit = true;
        }

    }
}
