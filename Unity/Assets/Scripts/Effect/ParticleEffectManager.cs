using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectManager : Singleton<ParticleEffectManager>
{
    private ObjectPoolManager objectPoolManager;
    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }

    public void PlayOneShot(string prefabKey, Transform parent)
    {
        var go = objectPoolManager.Get(prefabKey);
        if (go.GetComponent<PoolableEffect>() == null)
        {
            go.AddComponent<PoolableEffect>();
        }

        go.transform.SetParent(parent, false);
        var ps = go.GetComponent<ParticleSystem>();
        if(ps == null)
        {
            return;
        }

        ps.Play();
        go.GetComponent<PoolableEffect>().Release();
    }
}
