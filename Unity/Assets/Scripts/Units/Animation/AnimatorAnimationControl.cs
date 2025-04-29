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
    private readonly static int hashAttack = Animator.StringToHash("Attack");
    private readonly static int hashSkill = Animator.StringToHash("Skill");
    private readonly static int hashDie = Animator.StringToHash("Die");
    private readonly static int hashAttackIndex = Animator.StringToHash("AttackIndex");
    private readonly static int hashAttackSpeed = Animator.StringToHash("AttackSpeed");


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

    private Dictionary<AnimationClipID, List<EventPair>> events = new Dictionary<AnimationClipID, List<EventPair>>();

    private EventComparer eventComparer = new EventComparer();

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
        switch (clipID)
        {
            case AnimationClipID.None:
                break;
            case AnimationClipID.Idle:
                break;
            case AnimationClipID.BattleIdle:
                break;
            case AnimationClipID.Run:
                break;
            case AnimationClipID.Attack:
                animator.SetFloat(hashAttackSpeed, speed);
                break;
            case AnimationClipID.Skill:
                break;
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

    private float GetProgress(AnimationClipID clipID)
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
}
