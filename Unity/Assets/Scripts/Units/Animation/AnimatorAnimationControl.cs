using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorAnimationControl : AnimationControl
{
    private readonly static int hashBattleIdle = Animator.StringToHash("BattleIdle");
    private readonly static int hashRun = Animator.StringToHash("Run");
    private readonly static int hashAttack = Animator.StringToHash("Attack");
    private readonly static int hashSkill = Animator.StringToHash("Skill");
    private readonly static int hashDie = Animator.StringToHash("Die");
    private readonly static int hashAttackIndex = Animator.StringToHash("AttackIndex");

    private class EventPair
    {
        public float normalizedTime;
        public Action ev;
        public bool isInvoked;

        public EventPair(float normalizedTime, Action ev)
        {
            this.normalizedTime = normalizedTime;
            this.ev = ev;
            isInvoked = false;
        }
    }

    public class EventComparer : IComparer<EventPair>
    {
        int IComparer<EventPair>.Compare(EventPair x, EventPair y)
        {
            return x.normalizedTime.CompareTo(y.normalizedTime);
        }
    }

    private Animator animator;

    private AnimatorController animatorController;

    private Dictionary<AnimationClipID, List<EventPair>> events = new Dictionary<AnimationClipID, List<EventPair>>();

    private EventComparer eventComparer = new EventComparer();

    [SerializeField]
    private int attackIndexLength = 0;

    private int attackIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animatorController = animator.runtimeAnimatorController as AnimatorController;
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

    public override void AddEvent(AnimationClipID clipID, float normalizedTime, Action action)
    {
        if (!events.ContainsKey(clipID))
        {
            events.Add(clipID, new List<EventPair>());
        }
        events[clipID].Add(new EventPair(normalizedTime, action));
        events[clipID].Sort(eventComparer);
    }

    public override void RemoveEvent(AnimationClipID clipID, Action action)
    {
        if (!events.ContainsKey(clipID))
        {
            return;
        }

        for (int i = 0; i < events[clipID].Count; ++i)
        {
            if (events[clipID][i].ev == action)
            {
                events[clipID].RemoveAt(i);
                break;
            }
        }
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

        if (events.ContainsKey(clipID))
        {
            for (int i = 0; i < events[clipID].Count; ++i)
            {
                events[clipID][i].isInvoked = false;
            }
        }

        animator.ResetTrigger(hashBattleIdle);
        animator.ResetTrigger(hashRun);
        animator.ResetTrigger(hashAttack);
        animator.ResetTrigger(hashSkill);
        animator.ResetTrigger(hashDie);

        CurrentClip = clipID;

        switch (CurrentClip)
        {
            case AnimationClipID.BattleIdle:
                animator.SetTrigger(hashBattleIdle);
                break;
            case AnimationClipID.Run:
                animator.SetTrigger(hashRun);
                break;
            case AnimationClipID.Attack:
                animator.SetTrigger(hashAttack);
                break;
            case AnimationClipID.Skill:
                animator.SetTrigger(hashSkill);
                break;
            case AnimationClipID.Die:
                animator.SetTrigger(hashDie);
                break;
        }
    }

    public override void SetSpeed(float speed)
    {
        animator.speed = speed;
    }

    public override void SetSpeed(AnimationClipID clipID, float speed)
    {
        if (!ContainsClip(clipID))
        {
            return;
        }

        int hash;
        switch (clipID)
        {
            case AnimationClipID.BattleIdle:
                hash = hashBattleIdle;
                break;
            case AnimationClipID.Run:
                hash = hashRun;
                break;
            case AnimationClipID.Attack:
                hash = hashAttack;
                break;
            case AnimationClipID.Skill:
                hash = hashSkill;
                break;
            case AnimationClipID.Die:
                hash = hashDie;
                break;
            default:
                return;
        }

        foreach (var animatorState in animatorController.layers[0].stateMachine.states)
        {
            if (animatorState.state.nameHash != hash)
            {
                continue;
            }
            var clip = animatorState.state.motion as AnimationClip;
            if (clip.length > 1f)
            {
                animatorState.state.speed = clip.length * speed;
            }
            else
            {
                animatorState.state.speed = speed;
            }
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

    private float GetProgress(AnimationClipID clipID)
    {
        int hash;
        switch (clipID)
        {
            case AnimationClipID.BattleIdle:
                hash = hashBattleIdle;
                break;
            case AnimationClipID.Run:
                hash = hashRun;
                break;
            case AnimationClipID.Attack:
                hash = hashAttack;
                break;
            case AnimationClipID.Skill:
                hash = hashSkill;
                break;
            case AnimationClipID.Die:
                hash = hashDie;
                break;
            default:
                return 1f;
        }

        var animationState = animator.GetCurrentAnimatorStateInfo(0);

        if (animationState.normalizedTime > 1f)
        {
            return 1f;
        }

        return animationState.normalizedTime;
    }

    private void ProcessEvent()
    {
        var currentClip = CurrentClip;
        if (!events.ContainsKey(currentClip))
        {
            return;
        }
        float progress = GetProgress(currentClip);

        for (int i = 0; i < events[currentClip].Count; ++i)
        {
            var pair = events[currentClip][i];

            if (progress < pair.normalizedTime)
            {
                break;
            }
            if (pair.isInvoked)
            {
                continue;
            }
            pair.isInvoked = true;
            pair.ev.Invoke();
        }
    }

    public override bool ContainsClip(AnimationClipID clipID)
    {
        switch (clipID)
        {
            case AnimationClipID.BattleIdle:
                return animator.HasState(0, hashBattleIdle);
            case AnimationClipID.Run:
                return animator.HasState(0, hashRun);
            case AnimationClipID.Attack:
                return animator.HasState(0, hashAttack);
            case AnimationClipID.Skill:
                return animator.HasState(0, hashSkill);
            case AnimationClipID.Die:
                return animator.HasState(0, hashDie);
        }
        return false;
    }

    public void NextWeaponIndex()
    {
        if (attackIndexLength > 0)
        {
            attackIndex = (attackIndex + 1) % attackIndexLength;
            animator.SetInteger(hashAttackIndex, attackIndex);
        }
    }
}
