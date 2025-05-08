using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MineDefence : MonoBehaviour, IObjectPoolGameObject
{
    [field: SerializeField]
    public SerializedDictionary<UnitTypes, Transform> UnitSpawnPoints { get; private set; }

    [field: SerializeField]
    public Transform[] MonsterSpawnPoints { get; private set; }

    [field: SerializeField]
    public Transform[] MonsterGoals { get; private set; }

    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release()
    {
        ObjectPool.Release(gameObject);
    }
}
