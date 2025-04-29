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
        var effect = go.GetComponent<PoolableEffect>();
        if (effect == null)
        {
            effect = go.AddComponent<PoolableEffect>();
        }

        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        var ps = go.GetComponent<ParticleSystem>();
        if(ps == null)
        {
            effect.Release();
        }
        else
        {
            effect.PlayAndRelease(ps);
        }
    }
    public PoolableEffect PlayBuffEffect(string prefabKey, Transform parent = null)
    {
        GameObject go = objectPoolManager.Get(prefabKey);
        if (go == null) return null;

        var effect = go.GetComponent<PoolableEffect>();
        if (effect == null)
        {
            effect = go.AddComponent<PoolableEffect>();
        }

        go.transform.SetParent(parent, false);
        go.transform.localRotation = Quaternion.identity;

        var ps = go.GetComponent<ParticleSystem>()
                 ?? go.GetComponentInChildren<ParticleSystem>();

        if (ps != null)
        {
            ps.loop = true;
            ps.Play();
        }
        return effect;
    }

    public void StopBuffEffect(PoolableEffect effect)
    {
        if (effect == null)
        {
            return;
        }
        effect.Release();
    }

    public void PlayBuffEffect(string prefabKey, Transform parent, float duration)
    {
        var effect = PlayBuffEffect(prefabKey, parent);
        if (effect != null)
        {
            StartCoroutine(StopAfter(duration, effect));
        }
    }

    private IEnumerator StopAfter(float duration, PoolableEffect effect)
    {
        yield return new WaitForSeconds(duration);
        effect.Release();
    }
}
