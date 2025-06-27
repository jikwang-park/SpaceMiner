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
        Walk,
    }

    protected class EventPair
    {
        public float normalizedTime;
        public System.Action ev;
        public bool isInvoked;

        public EventPair(float normalizedTime, System.Action ev)
        {
            this.normalizedTime = normalizedTime;
            this.ev = ev;
            isInvoked = false;
        }
    }

    protected class EventComparer : IComparer<EventPair>
    {
        int IComparer<EventPair>.Compare(EventPair x, EventPair y)
        {
            return x.normalizedTime.CompareTo(y.normalizedTime);
        }
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
