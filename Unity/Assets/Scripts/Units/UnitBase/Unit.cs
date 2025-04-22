using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;




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

    [SerializeField]
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
            if (targetPos != null && targetDistance <= unitStats.range)
                return true;

            return false;
        }
    }
    public float targetDistance;

    public BehaviorTree<Unit> behaviorTree;

    public float attackUsingTime = 0.25f;

    public int unitAliveCount = 0;

    [SerializeField]
    public UnitSkill unitSkill;

    public Transform targetPos;

    private bool isTargetInArea = false;

    private int lane = 0;

    private int currentUnitNum = 0;

    public float maxDis = 6.0f;
    public float minDis = 4.0f;


    public float lastSkillUsedTime;

    public Grade currentGrade;

    public bool isAutoSkillMode;
    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        stageManger = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        isAutoSkillMode = true;
    }

    private void Start()
    {
    }

    

    public bool IsUnitExeedMonsterPosition
    {
        get
        {
            if (targetPos == null)
                return true;


            return transform.position.z < targetPos.position.z;
        }
    }

    public bool IsDead
    {
        get
        {
            if (unitStats.Hp <= 0)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsTargetDead
    {
        get
        {
            if ((targetPos == null || !targetPos.gameObject.activeSelf))
            {
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

    public bool IsUnitAliveFront
    {
        get
        {
            if (stageManger.UnitPartyManager.IsUnitExistFront(currentUnitType))
            {
                return true;
            }
            return false;
        }
    }
    float thisAndFrontUnitDistance;
    public bool IsUnitFar
    {
        get
        {
            var frontUnit = stageManger.UnitPartyManager.GetFrontUnit(currentUnitType);
            thisAndFrontUnitDistance = frontUnit.transform.position.z - transform.position.z;
            if (thisAndFrontUnitDistance > 5f)
                return true;

            return false;
        }
    }


    public bool IsTankerCanUseSkill
    {
        get
        {
            if (targetPos == null || Time.time < unitSkill.coolTime + lastSkillUsedTime)
                return false;

            return true;
        }
    }

    public bool IsDealerCanUseSkill
    {
        get
        {
            if (targetPos == null || targetDistance > unitStats.range ||
                    Time.time < unitSkill.coolTime + lastSkillUsedTime)
                return false;

            return true;
        }
    }

    public bool IsHealerCanUseSkill
    {
        get
        {
            if (unitSkill.targetList.Count == 0 ||
               Time.time < unitSkill.coolTime + lastSkillUsedTime)
                return false;

            foreach (var unit in unitSkill.targetList)
            {
                var targetStats = unit.unitStats;
                if ((targetStats.Hp.DivideToFloat(targetStats.maxHp)) * 100f <= stageManger.UnitPartyManager.buttonManager.currentValue)
                {
                    return true;
                }

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
    //
    public bool IsFrontLine
    {
        get
        {
            return !stageManger.UnitPartyManager.IsUnitExistFront(currentUnitType);
        }
    }
    public bool IsSafeDistance
    {
        get
        {
            bool isFront = !stageManger.UnitPartyManager.IsUnitExistFront(currentUnitType);
            bool isBack = !stageManger.UnitPartyManager.IsUnitExistBack(currentUnitType);

            if (isFront && isBack)
                return true;

            bool isBackSafe = true;
            bool isFrontSafe = true;

            if (!isBack)
            {
                var backUnit = stageManger.UnitPartyManager.GetBackUnit(currentUnitType);
                float distance = (transform.position.z - backUnit.transform.position.z);

                isBackSafe = distance <= (maxDis * ((int)backUnit.currentUnitType - (int)currentUnitType));

            }

            if (!isFront)
            {
                var frontUnit = stageManger.UnitPartyManager.GetFrontUnit(currentUnitType);
                float distance = (frontUnit.transform.position.z - transform.position.z);
                isFrontSafe = (distance >= minDis * (((int)currentUnitType) - (int)frontUnit.currentUnitType));

            }


            return (isBackSafe && isFrontSafe);

        }
    }




    public float lastAttackTime;


    private bool isMonsterSpawn;


    public void SetData(SoldierTable.Data data, UnitTypes type)
    {
        unitStats.SetData(data, type);
        currentUnitType = type;
        currentGrade = data.Grade;
        switch (currentUnitType)
        {
            case UnitTypes.Tanker:
                if (unitSkill == null)
                {
                    unitSkill = gameObject.AddComponent<TankerSkill>();
                }
                unitSkill.TankerInit(currentUnitType, currentGrade);
                break;
            case UnitTypes.Dealer:
                if (unitSkill == null)
                {
                    unitSkill = gameObject.AddComponent<DealerSkill>();
                }
                unitSkill.DealerInit(currentUnitType, currentGrade);
                break;
            case UnitTypes.Healer:
                if (unitSkill == null)
                {
                    unitSkill = gameObject.AddComponent<HealerSkill>();
                }
                unitSkill.HealerInit(currentUnitType, currentGrade);
                break;
        }
        unitSkill.currentType = currentUnitType;
        unitSkill.currentSkillGrade = currentGrade;
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
                // 250403 HKY - 한 유닛에서 전체 유닛 순회하지 않도록 수정

                targetDistance = targetPosition.position.z - transform.position.z;
                targetPos = targetPosition;


                targetPos.GetComponent<DestructedDestroyEvent>().OnDestroyed += (_) => targetPos = null;
                return targetPos;

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
        yield return new WaitForSeconds(unitStats.attackSpeed / 100);
        currentStatus = UnitStatus.Wait;
    }

    private IEnumerator HealerSkillTimer()
    {
        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(0.25f);
        lastAttackTime = Time.time;
        currentStatus = UnitStatus.Wait;
    }


    private IEnumerator DealerSkillTimer()
    {

        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(0.25f);
        currentStatus = UnitStatus.Wait;
    }
    private IEnumerator TankerSkillTimer()
    {
        unitSkill.GetTarget();
        unitSkill.ExecuteSkill();
        yield return new WaitForSeconds(0.25f);
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

    public void GetSaveStats(UnitUpgradeTable.UpgradeType type, int level)
    {
        float value = DataTableManager.UnitUpgradeTable.GetData(type).Value;
        float stats = 0;

        for (int i = 1; i <= level; ++i)
        {
            stats += value * i;
        }
        unitStats.AddStats(type, stats);
    }

    public void GetSaveBuildingStats(BuildingTable.BuildingType type, int level)
    {
        var data = DataTableManager.BuildingTable.GetDatas(type);
        float buildingStats = 0;
        for (int i = 0; i <= level; ++i)
        {
            buildingStats = data[i].Value;
        }
        unitStats.AddBuildingStats(type, buildingStats);

    }
}
