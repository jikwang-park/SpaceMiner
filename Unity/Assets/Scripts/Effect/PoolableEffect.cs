using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolableEffect : MonoBehaviour, IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release()
    {
        var ps = GetComponent<ParticleSystem>() ?? GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        if (ObjectPool != null)
        {
            ObjectPool.Release(this.gameObject);
        }
    }
}
