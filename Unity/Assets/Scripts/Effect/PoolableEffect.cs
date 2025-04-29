using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableEffect : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }
    private ParticleSystem ps;
    private Coroutine releaseCoroutine;

    public void PlayAndRelease(ParticleSystem particleSystem)
    {
        ps = particleSystem;
        ps.Play();
        if (releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
        }
        releaseCoroutine = StartCoroutine(ReleaseWhenDone());
    }
    private IEnumerator ReleaseWhenDone()
    {
        yield return new WaitUntil(() => !ps.IsAlive(true));
        Release();
    }
    public void Release()
    {
        if (ObjectPool != null)
        {
            ObjectPool.Release(this.gameObject);
        }
    }
    public void ImmediateRelease()
    {
        if(releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
        }
        Release();
    }
}
