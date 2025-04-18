using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationController : AnimationControl
{
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

    private const string battleidle = "Battle_Idle_01";
    private const string idle = "Idle";
    private const string run = "Run";
    private const string attack = "Attack_01";
    private const string skill = "Skill_01";
    private const string die = "Die";

    private Animation animations;

    [SerializeField]
    private SerializedDictionary<AnimationClipID, string> animationDict;

    [SerializeField]
    private float fadeLength = 0.2f;

    private Dictionary<AnimationClipID, List<EventPair>> events = new Dictionary<AnimationClipID, List<EventPair>>();

    private EventComparer eventComparer = new EventComparer();

    private void Awake()
    {
        animations = GetComponent<Animation>();
    }

    private void Start()
    {
        animations.wrapMode = WrapMode.Loop;
        foreach (AnimationState state in animations)
        {
            switch (state.name)
            {
                case battleidle:
                    animationDict.Add(AnimationClipID.BattleIdle, battleidle);
                    state.wrapMode = WrapMode.Loop;
                    break;
                case idle:
                    animationDict.Add(AnimationClipID.Idle, idle);
                    state.wrapMode = WrapMode.Loop;
                    break;
                case run:
                    animationDict.Add(AnimationClipID.Run, run);
                    state.wrapMode = WrapMode.Loop;
                    break;
                case attack:
                    animationDict.Add(AnimationClipID.Attack, attack);
                    state.wrapMode = WrapMode.ClampForever;
                    break;
                case skill:
                    animationDict.Add(AnimationClipID.Skill, skill);
                    state.wrapMode = WrapMode.ClampForever;
                    break;
                case die:
                    animationDict.Add(AnimationClipID.Die, die);
                    state.wrapMode = WrapMode.ClampForever;
                    break;
            }
        }
        CurrentClip = AnimationClipID.None;
    }

    private void Update()
    {
        if (!animations.isPlaying)
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
        if (!animationDict.ContainsKey(clipID))
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


        CurrentClip = clipID;
        animations.CrossFade(animationDict[clipID], fadeLength);
    }

    public override void SetSpeed(float speed)
    {
        foreach (AnimationState state in animations)
        {
            state.speed = speed / state.length;
        }
    }

    public override void SetSpeed(AnimationClipID clipID, float speed)
    {
        if (!animationDict.ContainsKey(clipID))
        {
            return;
        }
        var state = animations[animationDict[clipID]];
        state.normalizedSpeed = speed;
    }


    public override void SetLoop(AnimationClipID clipID, bool isLoop)
    {
        if (!animationDict.ContainsKey(clipID))
        {
            return;
        }

        if (isLoop)
        {
            animations[animationDict[clipID]].wrapMode = WrapMode.Loop;
        }
        else
        {
            animations[animationDict[clipID]].wrapMode = WrapMode.ClampForever;
        }
    }

    public override void Stop()
    {
        animations.Stop();
    }

    private float GetProgress(AnimationClipID clipID)
    {
        if (!animationDict.ContainsKey(clipID))
        {
            return 1f;
        }
        if (!animations.isPlaying)
        {
            return 1f;
        }

        var animationState = animations[animationDict[clipID]];

        if (animationState.wrapMode == WrapMode.ClampForever
            && animationState.normalizedTime > 1f)
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
}
