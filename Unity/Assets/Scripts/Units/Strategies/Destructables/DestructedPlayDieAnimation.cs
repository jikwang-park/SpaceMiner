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

        waitDie = new WaitUntil(IsEnd);
    }

    public void OnDestruction(GameObject Attacker)
    {
        StartCoroutine(CoDie());
    }

    private bool IsEnd()
    {
        return animationControl.GetProgress(AnimationControl.AnimationClipID.Die) >= 1f;
    }

    private IEnumerator CoDie()
    {
        animationControl.Play(AnimationControl.AnimationClipID.Die);
        yield return waitDie;
        gameObject.GetComponent<IObjectPoolGameObject>().Release();
    }
}
