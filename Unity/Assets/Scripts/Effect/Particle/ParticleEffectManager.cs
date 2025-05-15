using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleEffectManager : Singleton<ParticleEffectManager>
{
    private ObjectPoolManager objectPoolManager;
    private List<PoolableEffect> playingEffects = new List<PoolableEffect>();
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
        effect.OnRelease += OnEffectReleased;
        playingEffects.Add(effect);
        effect.ResetTransform();

        go.transform.SetParent(parent, false);
        var ps = go.GetComponent<ParticleSystem>();
        if(ps == null)
        {
            effect.Release();
        }
        else
        {
            effect.OnRelease += ((e) => playingEffects.Remove(e));
            effect.PlayAndRelease(ps);
        }
    }
    public void PlayOneShot(string prefabKey, Vector3 position)
    {
        var go = objectPoolManager.Get(prefabKey);
        var effect = go.GetComponent<PoolableEffect>();
        if (effect == null)
        {
            effect = go.AddComponent<PoolableEffect>();
        }
        effect.OnRelease += OnEffectReleased;
        playingEffects.Add(effect);
        effect.ResetTransform();

        go.transform.position += position;
        var ps = go.GetComponent<ParticleSystem>();
        if (ps == null)
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
        effect.ResetTransform();
        playingEffects.Add(effect);
        go.transform.SetParent(parent, false);

        var ps = go.GetComponent<ParticleSystem>()
                 ?? go.GetComponentInChildren<ParticleSystem>();

        if (ps != null)
        {
            ps.loop = true;
            ps.Play();
        }

        effect.OnRelease += OnEffectReleased;
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
        StopBuffEffect(effect);
    }
    private void OnEffectReleased(PoolableEffect effect)
    {
        effect.OnRelease -= OnEffectReleased;
        playingEffects.Remove(effect);
    }
    public void ClearAllEffects()
    {
        foreach(var effect in playingEffects.Where(e => e != null).ToArray())
        {
            effect.ImmediateRelease();
        }
        playingEffects.Clear();
    }
}
