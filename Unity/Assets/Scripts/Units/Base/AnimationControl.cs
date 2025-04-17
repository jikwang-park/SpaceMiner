using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationControl : MonoBehaviour
{
    public enum AnimationClipID
    {
        Idle,
        BattleIdle,
        Run,
        Attack,
        Skill,
        Die,
    }

    public abstract void Play(AnimationClipID clipID);

    public abstract void Play(AnimationClipID clipID, bool isLoop);

    public abstract void SetSpeed(AnimationClipID clipID, float speed);
    public abstract void SetSpeed(float speed);

    public abstract void SetLoop(AnimationClipID clipID, bool isLoop);

    public abstract float GetProgress(AnimationClipID clipID);
}
