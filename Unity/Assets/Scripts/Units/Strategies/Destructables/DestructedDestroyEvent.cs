using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedDestroyEvent : MonoBehaviour, IDestructable
{
    public event Action OnDestroyed;
    public void OnDestruction(GameObject Attacker)
    {
        OnDestroyed?.Invoke();
    }
}
