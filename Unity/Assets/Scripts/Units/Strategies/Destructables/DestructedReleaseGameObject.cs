using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedReleaseGameObject : MonoBehaviour, IDestructable
{
    public void OnDestruction(GameObject Attacker)
    {
        gameObject.GetComponent<IObjectPoolGameObject>().Release();
    }
}
