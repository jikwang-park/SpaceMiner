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

    protected Dictionary<AnimationClipID, List<EventPair>> events = new Dictionary<AnimationClipID, List<EventPair>>();

    protected EventComparer eventComparer = new EventComparer();

    public abstract bool ContainsClip(AnimationClipID clipID);

    public abstract void Play(AnimationClipID clipID);

    public abstract void Play(AnimationClipID clipID, bool isLoop);

    public abstract void Stop();

    public abstract void SetSpeed(AnimationClipID clipID, float speed);
    public abstract void SetSpeed(float speed);

    public virtual void AddEvent(AnimationClipID clipID, float normalizedTime, System.Action action)
    {
        if (!events.ContainsKey(clipID))
        {
            events.Add(clipID, new List<EventPair>());
        }
        events[clipID].Add(new EventPair(normalizedTime, action));
        events[clipID].Sort(eventComparer);
    }

    public virtual void RemoveEvent(AnimationClipID clipID, System.Action action)
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

    protected virtual void ProcessEvent()
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

    protected abstract float GetProgress(AnimationClipID clipID);

    public abstract void SetLoop(AnimationClipID clipID, bool isLoop);

    protected void ResetEvent(AnimationClipID clipID)
    {
        if (events.ContainsKey(clipID))
        {
            for (int i = 0; i < events[clipID].Count; ++i)
            {
                events[clipID][i].isInvoked = false;
            }
        }
    }
}
