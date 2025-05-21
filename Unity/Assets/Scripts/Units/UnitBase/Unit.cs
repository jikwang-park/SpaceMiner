using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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

    [field: SerializeField]
    public Transform LeftHandPosition { get; private set; }

    [field: SerializeField]
    public Transform RightHandPosition { get; private set; }

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

    public bool IsTargetInRange
    {
        get
        {
            return TargetDistance <= unitStats.range;
        }
    }

    private bool isSkillInQueue;

    public bool HasTarget { get; private set; }

    private bool skillExecuted;

    private UnitColorPaletteSetter colorSetter;

    private void Awake()
    {
        AnimationControl = GetComponent<AnimationControl>();
        unitStats = GetComponent<UnitStats>();
        colorSetter = GetComponent<UnitColorPaletteSetter>();
    }

    private void Start()
    {
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Attack, attackTime, OnAttack);
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Attack, 1f, OnAttackEnd);
        if (UnitTypes == UnitTypes.Healer)
        {
            AnimationControl.AddEvent(AnimationControl.AnimationClipID.Idle, skillTime, ExecuteSkill);
            AnimationControl.AddEvent(AnimationControl.AnimationClipID.Idle, 1f, OnSkillEnd);
        }
        else
        {
            AnimationControl.AddEvent(AnimationControl.AnimationClipID.Skill, skillTime, ExecuteSkill);
            AnimationControl.AddEvent(AnimationControl.AnimationClipID.Skill, 1f, OnSkillEnd);
        }
        AnimationControl.AddEvent(AnimationControl.AnimationClipID.Die, 1f, OnEnd);
    }


    private void OnEnable()
    {
        if (StageManager is null)
        {
            StageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }
        GetComponent<DestructedDestroyEvent>().OnDestroyed += (_) => SetStatus(Status.Dead);
    }

    private void Update()
    {
        if (UnitStatus == Status.Dead)
        {
            return;
        }

        if (StageManager.IngameStatus == IngameStatus.Mine)
        {
            if (HasTarget)
            {
                var targetDisplacement = target.position - transform.position;
                targetDisplacement.y = 0f;
                TargetDistance = Vector3.Magnitude(targetDisplacement);
                if (TargetDistance > unitStats.range)
                {
                    HasTarget = false;
                    target.GetComponent<DestructedDestroyEvent>().OnDestroyed -= OnTargetDie;
                    TargetDistance = float.MaxValue;
                }
            }

            if (!HasTarget && StageManager.StageMonsterManager.MonsterCount > 0)
            {
                var monster = StageManager.StageMonsterManager.GetMonster(transform.position, unitStats.range);
                if (monster is not null)
                {
                    HasTarget = true;
                    target = monster.transform;
                    var targetDisplacement = target.position - transform.position;
                    targetDisplacement.y = 0f;
                    TargetDistance = Vector3.Magnitude(targetDisplacement);
                    target.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnTargetDie;
                }
            }

            if (HasTarget)
            {
                var direction = target.position - transform.position;
                direction.y = 0f;
                direction.Normalize();
                direction = Vector3.Lerp(transform.forward, direction, Time.deltaTime * unitStats.moveSpeed).normalized;
                transform.forward = direction;
            }
        }
        else
        {
            if (HasTarget)
            {
                TargetDistance = target.position.z - transform.position.z;
            }
            else if (StageManager.StageMonsterManager.MonsterCount > 0)
            {
                HasTarget = true;
                var monster = StageManager.StageMonsterManager.GetMonsters(1)[0];
                target = monster;
                TargetDistance = target.position.z - transform.position.z;
                target.GetComponent<DestructedDestroyEvent>().OnDestroyed += OnTargetDie;
            }
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

        AnimationControl.SetSpeed(AnimationControl.AnimationClipID.Attack, 1f / UnitCombatPowerCalculator.statsDictionary[data.UnitType].coolDown);
        if (data.UnitType == UnitTypes.Healer)
        {
            AnimationControl.SetSpeed(AnimationControl.AnimationClipID.Idle, 1f / UnitCombatPowerCalculator.statsDictionary[data.UnitType].coolDown);
        }
        else
        {
            AnimationControl.SetSpeed(AnimationControl.AnimationClipID.Skill, 1f / UnitCombatPowerCalculator.statsDictionary[data.UnitType].coolDown);
        }

        UnitTypes = data.UnitType;
        Grade = data.Grade;
        colorSetter.SetColor(Grade, UnitTypes);
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
        StageManager.StageUiManager.UnitUiManager.SetUnitHpBar(UnitTypes, unitStats);

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
        if (UnitStatus == Status.SkillUsing)
        {
            skillExecuted = true;
            Skill.ExecuteSkill();
            SoundManager.Instance.PlaySFX(UnitTypes.ToString() + "SkillSFX");
        }
    }



    private void OnAttackEnd()
    {
        if ((!HasTarget || !IsTargetInRange)
            && StageManager.IngameStatus != IngameStatus.Mine)
        {
            SetStatus(Status.Run);
        }
        else
        {
            SetStatus(Status.Wait);
        }
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
                if ((!HasTarget || !IsTargetInRange)
                    && StageManager.IngameStatus != IngameStatus.Mine)
                {
                    SetStatus(Status.Run);
                    return;
                }
                AnimationControl?.Play(AnimationControl.AnimationClipID.BattleIdle);
                break;
            case Status.Attacking:
                lastAttackTime = Time.time;
                if (UnitTypes == UnitTypes.Tanker)
                {
                    ((AnimatorAnimationControl)AnimationControl).NextWeaponIndex();
                }
                AnimationControl?.Play(AnimationControl.AnimationClipID.Attack);
                break;
            case Status.SkillUsing:
                lastSkillTime = Time.time;
                skillExecuted = false;
                if (UnitTypes == UnitTypes.Healer)
                {
                    AnimationControl?.Play(AnimationControl.AnimationClipID.Idle);
                }
                else
                {
                    AnimationControl?.Play(AnimationControl.AnimationClipID.Skill);
                }
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
                if ((!HasTarget || !IsTargetInRange)
                    && StageManager.IngameStatus != IngameStatus.Mine)
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
                if (HasTarget && Time.time > lastAttackTime + unitStats.coolDown)
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

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }

    public void ResetStatus()
    {
        HasTarget = false;
        target = null;
        if (UnitStatus == Status.SkillUsing)
        {
            if (!skillExecuted && UnitTypes != UnitTypes.Dealer)
            {
                Skill.ExecuteSkill();
                SoundManager.Instance.PlaySFX(UnitTypes.ToString() + "SkillSFX");
            }
            lastSkillTime = Time.time;
        }
        SetStatus(Status.Wait);
    }
}
