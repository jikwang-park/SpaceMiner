using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffect : MonoBehaviour, IDestructable
{
    public void OnDestruction(GameObject Attacker)
    {
        ParticleEffectManager.Instance.PlayOneShot("MonsterDeathEffect", gameObject.transform.position);
    }

}
