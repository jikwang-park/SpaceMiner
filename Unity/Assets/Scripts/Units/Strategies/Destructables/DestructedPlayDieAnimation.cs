using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedPlayDieAnimation : MonoBehaviour, IDestructable
{
    private AnimationControl animationControl;
    private WaitUntil waitDie;


    private void Awake()
    {
        animationControl = GetComponent<AnimationControl>();
    }

    private void Start()
    {
        animationControl.AddEvent(AnimationControl.AnimationClipID.Die, 1f, OnEnd);
    }

    public void OnDestruction(GameObject Attacker)
    {
        animationControl.Play(AnimationControl.AnimationClipID.Die);
    }

    private void OnEnd()
    {
        gameObject.GetComponent<IObjectPoolGameObject>().Release();
    }
}
