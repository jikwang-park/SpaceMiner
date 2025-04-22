using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileShooter : MonoBehaviour, IObjectPoolGameObject
{
    [SerializeField]
    private Transform shootingPoint;
    [SerializeField]
    private GameObject bulletPrefabs;
    [SerializeField]
    private Unit unit;
    public IObjectPool<GameObject> ObjectPool { get ; set ; }

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        
    }

    public void Fire(Transform startPos , Transform hitPos , Attack damage)
    {
        

    }


    public void Release()
    {
        ObjectPool.Release(gameObject);
    }
}
