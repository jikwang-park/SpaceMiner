using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorAnimationControl : AnimationControl
{
    private readonly static int hashBattleIdle = Animator.StringToHash("BattleIdle");
    private readonly static int hashRun = Animator.StringToHash("Run");
    private readonly static int hashWalk = Animator.StringToHash("Walk");
    private readonly static int hashAttack = Animator.StringToHash("Attack");
    private readonly static int hashSkill = Animator.StringToHash("Skill");
    private readonly static int hashDie = Animator.StringToHash("Die");
    private readonly static int hashAttackIndex = Animator.StringToHash("AttackIndex");
    private readonly static int hashAttackSpeed = Animator.StringToHash("AttackSpeed");
    private readonly static int hashSkillSpeed = Animator.StringToHash("SkillSpeed");

    private readonly static Dictionary<AnimationClipID, int> dictHash = new Dictionary<AnimationClipID, int>()
    {
        { AnimationClipID.BattleIdle, hashBattleIdle },
        { AnimationClipID.Run, hashRun },
        { AnimationClipID.Walk, hashWalk },
        { AnimationClipID.Attack, hashAttack },
        { AnimationClipID.Skill, hashSkill } ,
        { AnimationClipID.Die, hashDie },
    };

    private Animator animator;

    [SerializeField]
    private int attackIndexLength = 0;

    private int attackIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        CurrentClip = AnimationClipID.None;
    }

    private void Update()
    {
        if (!animator.enabled)
        {
            return;
        }
        ProcessEvent();
    }

    public override void Play(AnimationClipID clipID)
    {
        Play(clipID, false);
    }

    public override void Play(AnimationClipID clipID, bool shouldForce)
    {
        if (CurrentClip == clipID && !shouldForce)
        {
            return;
        }

        if (!animator.enabled)
        {
            animator.enabled = true;
        }

        ResetEvent(clipID);

        CurrentClip = clipID;

        ResetTrigger();

        animator.SetTrigger(dictHash[CurrentClip]);
    }

    public override void SetSpeed(float speed)
    {
        animator.speed = speed;
    }

    public override void SetSpeed(AnimationClipID clipID, float speed)
    {
        switch (clipID)
        {
            case AnimationClipID.Attack:
                animator.SetFloat(hashAttackSpeed, speed);
                break;
            case AnimationClipID.Skill:
                animator.SetFloat(hashSkillSpeed, speed);
                break;
            case AnimationClipID.None:
            case AnimationClipID.Idle:
            case AnimationClipID.BattleIdle:
            case AnimationClipID.Run:
            case AnimationClipID.Die:
                break;
        }
    }


    [Obsolete("Not use in AnimatorController")]
    public override void SetLoop(AnimationClipID clipID, bool isLoop)
    {
    }

    public override void Stop()
    {
        CurrentClip = AnimationClipID.None;
        animator.enabled = false;
    }

    protected override float GetProgress(AnimationClipID clipID)
    {
        var currentClips = animator.GetCurrentAnimatorClipInfo(0);
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        var nextClips = animator.GetNextAnimatorClipInfo(0);
        var nextState = animator.GetNextAnimatorStateInfo(0);

        var clipIDString = clipID.ToString();

        foreach (var currentClip in currentClips)
        {
            if (currentClip.clip.name.Contains(clipIDString))
            {
                return currentState.normalizedTime;
            }
        }

        foreach (var nextClip in nextClips)
        {
            if (nextClip.clip.name.Contains(clipIDString))
            {
                return nextState.normalizedTime;
            }
        }

        return 0f;
    }

    public override bool ContainsClip(AnimationClipID clipID)
    {
        string clipIDString = clipID.ToString();
        if (clipID == AnimationClipID.Attack && attackIndexLength > 0)
        {
            return animator.HasState(0, Animator.StringToHash($"{clipIDString}0"));
        }
        else
        {
            return animator.HasState(0, Animator.StringToHash(clipIDString));
        }
    }

    public void NextWeaponIndex()
    {
        if (attackIndexLength > 0)
        {
            animator.SetInteger(hashAttackIndex, attackIndex);
            attackIndex = (attackIndex + 1) % attackIndexLength;
        }
    }

    private void ResetTrigger()
    {
        animator.ResetTrigger(hashRun);
        animator.ResetTrigger(hashBattleIdle);
    }
}
