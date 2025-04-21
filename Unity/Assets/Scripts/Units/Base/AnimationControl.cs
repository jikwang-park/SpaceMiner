using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationControl : MonoBehaviour
{
    public enum AnimationClipID
    {
        None = -1,
        Idle,
        BattleIdle,
        Run,
        Attack,
        Skill,
        Die,
    }

    public AnimationClipID CurrentClip { get; protected set; }

    public abstract bool ContainsClip(AnimationClipID clipID);

    public abstract void Play(AnimationClipID clipID);

    public abstract void Play(AnimationClipID clipID, bool isLoop);

    public abstract void Stop();

    public abstract void SetSpeed(AnimationClipID clipID, float speed);
    public abstract void SetSpeed(float speed);

    public abstract void AddEvent(AnimationClipID clipID, float normalizedTime, System.Action action);
    public abstract void RemoveEvent(AnimationClipID clipID, System.Action action);

    public abstract void SetLoop(AnimationClipID clipID, bool isLoop);
}
