using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationController : AnimationControl
{
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
    private float targetWeight = 0.5f;

    [SerializeField]
    private float fadeLength = 0.2f;

    private void Awake()
    {
        animations = GetComponent<Animation>();
    }

    private void Start()
    {
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
                    state.wrapMode = WrapMode.Once;
                    break;
                case skill:
                    animationDict.Add(AnimationClipID.Skill, skill);
                    state.wrapMode = WrapMode.Once;
                    break;
                case die:
                    animationDict.Add(AnimationClipID.Die, die);
                    state.wrapMode = WrapMode.Once;
                    break;
            }
        }
    }

    //public void AddEvent(AnimationClipID clipID, System.Action<float> action)
    //{
    //    if (!animationDict.ContainsKey(clipID))
    //    {
    //        return;
    //    }
    //    if (!animationEvents.ContainsKey(clipID))
    //    {
    //        animationEvents.Add(clipID, null);
    //    }
    //    animationEvents[clipID] += action;
    //}

    //public void RemoveEvent(AnimationClipID clipID, System.Action<float> action)
    //{
    //    if (!animationDict.ContainsKey(clipID))
    //    {
    //        return;
    //    }
    //    if (!animationEvents.ContainsKey(clipID))
    //    {
    //        return;
    //    }

    //    animationEvents[clipID] -= action;
    //}

    public override void Play(AnimationClipID clipID)
    {
        if (!animationDict.ContainsKey(clipID))
        {
            return;
        }
        if (animations.IsPlaying(animationDict[clipID]))
        {
            return;
        }

        animations.Blend(animationDict[clipID], targetWeight, fadeLength);
    }

    public override void Play(AnimationClipID clipID, bool isLoop)
    {
        Play(clipID);
        SetLoop(clipID, isLoop);
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

        foreach (AnimationState state in animations)
        {
            if (state.name == animationDict[clipID])
            {
                state.speed = speed / state.length;
                break;
            }
        }
    }

    public override float GetProgress(AnimationClipID clipID)
    {
        if (!animationDict.ContainsKey(clipID))
        {
            return 1f;
        }
        if (!animations.IsPlaying(animationDict[clipID]))
        {
            return 1f;
        }

        return animations[animationDict[clipID]].normalizedTime;
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
            animations[animationDict[clipID]].wrapMode = WrapMode.Once;
        }
    }
}
