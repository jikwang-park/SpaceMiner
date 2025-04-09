using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Mine : MonoBehaviour, IObjectPoolGameObject
{
    [SerializeField]
    private Transform[] storages;

    [SerializeField]
    private Transform[] ores;

    [SerializeField]
    private Transform[] spawnPoints;

    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }

    public Transform GetOre(int index)
    {
        if (index < 0 || index >= ores.Length)
        {
            return null;
        }

        return ores[index];
    }

    public Transform GetStorage(int index)
    {
        if (index < 0 || index >= storages.Length)
        {
            return null;
        }

        return storages[index];
    }
}
