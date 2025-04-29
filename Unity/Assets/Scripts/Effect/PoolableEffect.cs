using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableEffect : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    private ParticleSystem ps;
    private Coroutine releaseCoroutine;
    private void Awake()
    {
        defaultPos = transform.localPosition;
        defaultRot = transform.localRotation;
    }
    public void ResetTransform()
    {
        transform.localPosition = defaultPos;
        transform.localRotation = defaultRot;
    }
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
        ResetTransform();
        if (ObjectPool != null)
        {
            ObjectPool.Release(gameObject);
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
