using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Unit : MonoBehaviour, IObjectPoolGameObject
{
    public enum Status
    {
        Wait,
        Attacking,
        SkillUsing,
        Dead,
        Run,
    }

    [field: SerializeField]
    public UnitTypes UnitTypes { get; private set; }

    [field: SerializeField]
    public Grade Grade { get; private set; }

    [SerializeField]
    private float attackTime = 0.3f;

    [SerializeField]
    private float skillTime = 0.2f;

    public Status UnitStatus { get; private set; }

    public IObjectPool<GameObject> ObjectPool { get; set; }

    public float TargetDistance { get; private set; }


    [field: SerializeField]
    public UnitSkillBase Skill { get; private set; }

    public StageManager StageManager { get; private set; }

    public AnimationControl AnimationControl { get; private set; }

    public UnitStats unitStats { get; private set; }

    public Transform target;

    public float lastAttackTime;

    public float lastSkillTime;

    public float SkillCoolTimeRatio
    {
        get
        {
            if (UnitStatus == Status.SkillUsing)
            {
                return 0f;
            }

            return Mathf.Min((Time.time - lastSkillTime) / Skill.CoolTime, 1f);
        }
    }

    public float RemainCooltime
    {
        get
        {
            if (UnitStatus == Status.SkillUsing)
            {
                return Skill.CoolTime;
            }
            return Mathf.Max(Skill.CoolTime + lastSkillTime - Time.time, 0f);
        }
    }

    public bool IsTargetInRange
    {
        get
        {
            return TargetDistance <= unitStats.range;
        }
    }

    private bool isSkillInQueue;

    public bool HasTarget { get; private set; }


    private void Awake()
    {
        AnimationControl = GetComponent<AnimationControl>();
        unitStats = GetComponent<UnitStats>();
    }

    private void Start()
    {
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Attack, attackTime, OnAttack);
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Attack, 1f, OnAttackEnd);
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Skill, skillTime, ExecuteSkill);
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Skill, 1f, OnSkillEnd);
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Die, 1f, OnEnd);


        StageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }


    private void OnEnable()
    {
        GetComponent<DestructedDestroyEvent>().OnDestroyed += (_) => SetStatus(Status.Dead);
    }

    private void Update()
    {
//baseSkill.UpdateCoolTime();
        if (UnitStatus == Status.Dead)
        {
            return;
        }

        if (HasTarget)
        {
            TargetDistance = target.position.z - transform.position.z;

        }
        else if (StageManager.StageMonsterManager.MonsterCount > 0)
        {
            HasTarget = true;
            var monster = StageManager.StageMonsterManager.GetMonsters(1)[0];
            target = monster;
            target.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnTargetDie;
        }
        if (Variables.isAutoSkillMode)
        {
            switch (UnitTypes)
            {
                case UnitTypes.Dealer:
                    if (HasTarget && IsTargetInRange)
                    {
                        EnqueueSkill();
                    }
                    break;
                case UnitTypes.Healer:
                    bool needHeal = StageManager.UnitPartyManager.NeedHealUnit();
                    if (needHeal)
                    {
                        EnqueueSkill();
                    }
                    break;
            }
        }

        UpdateStatus();
    }

    public void EnqueueSkill()
    {
        if (Time.time > Skill.CoolTime + lastSkillTime)
        {
            isSkillInQueue = true;
        }
    }

    public void SetData(SoldierTable.Data data)
    {
        unitStats.SetData(data, data.UnitType);
        UnitTypes = data.UnitType;
        Grade = data.Grade;
        switch (UnitTypes)
        {
            case UnitTypes.Tanker:
                Skill = new UnitTankerSkill();
                Skill.InitSkill(this);
                break;
            case UnitTypes.Dealer:
                Skill = new UnitDealerSkill();
                Skill.InitSkill(this);
                break;
            case UnitTypes.Healer:
                Skill = new UnitHealerSkill();
                Skill.InitSkill(this);
                break;
        }
    }

    private void OnTargetDie(DestructedDestroyEvent sender)
    {
        sender.OnDestroyed -= OnTargetDie;
        HasTarget = false;
        target = null;
    }

    private void OnSkillEnd()
    {
        if (UnitStatus == Status.SkillUsing)
        {
            lastSkillTime = Time.time;
        }
        SetStatus(Status.Wait);
    }

    private void ExecuteSkill()
    {
        Skill.ExecuteSkill();
    }

    private void OnAttackEnd()
    {
        SetStatus(Status.Wait);
    }

    private void OnAttack()
    {
        if (HasTarget)
        {
            unitStats.Execute(target.gameObject);
        }
    }

    private void OnEnd()
    {
        Destroy(gameObject);
    }

    private void SetStatus(Status status)
    {
        var previous = UnitStatus;
        UnitStatus = status;
        switch (status)
        {
            case Status.Wait:
                if (!HasTarget || !IsTargetInRange)
                {
                    SetStatus(Status.Run);
                    return;
                }
                AnimationControl?.Play(AnimationControl.AnimationClipID.BattleIdle);
                break;
            case Status.Attacking:
                lastAttackTime = Time.time;
                AnimationControl?.Play(AnimationControl.AnimationClipID.Attack);
                break;
            case Status.SkillUsing:
                AnimationControl?.Play(AnimationControl.AnimationClipID.Skill);
                break;
            case Status.Dead:
                AnimationControl?.Play(AnimationControl.AnimationClipID.Die);
                break;
            case Status.Run:
                AnimationControl?.Play(AnimationControl.AnimationClipID.Run);
                break;
        }
    }

    private void UpdateStatus()
    {
        switch (UnitStatus)
        {
            case Status.Wait:
                if (!HasTarget || !IsTargetInRange)
                {
                    SetStatus(Status.Run);
                    return;
                }
                if (isSkillInQueue)
                {
                    isSkillInQueue = false;
                    SetStatus(Status.SkillUsing);
                    return;
                }
                if (Time.time > lastAttackTime + unitStats.attackSpeed / 100f)
                {
                    SetStatus(Status.Attacking);
                }
                break;
            case Status.Run:
                if (HasTarget && IsTargetInRange)
                {
                    SetStatus(Status.Wait);
                    return;
                }
                if (isSkillInQueue)
                {
                    isSkillInQueue = false;
                    SetStatus(Status.SkillUsing);
                    return;
                }
                transform.position += Vector3.forward * Time.deltaTime * unitStats.moveSpeed;
                break;
        }
    }

    public void GetSaveStats(UnitUpgradeTable.UpgradeType type, int level)
    {
        float value = DataTableManager.UnitUpgradeTable.GetData(type).Value;
        float stats = 0;

        stats = value * level;
       
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

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }

    public void ResetStatus()
    {
        SetStatus(Status.Wait);
        HasTarget = false;
        target = null;
    }
}
