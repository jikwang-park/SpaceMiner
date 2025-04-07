using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : MonoBehaviour
{
    private const string objectPoolIDFormat = "ObjectPoolGameObject/{0}";

    [SerializeField]
    private string[] addressableAssetsNames;

    [SerializeField]
    private GameObject[] prefabs;

    private Dictionary<string, IObjectPool<GameObject>> gameObjectPools = new Dictionary<string, IObjectPool<GameObject>>();

    private void Awake()
    {
        for (int i = 0; i < addressableAssetsNames.Length; ++i)
        {
            if (!gameObjectPools.ContainsKey(addressableAssetsNames[i]))
            {
                CreateAddressableObjectPool(addressableAssetsNames[i]);
            }
        }
        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (!gameObjectPools.ContainsKey(prefabs[i].name))
            {
                ObjectPool<GameObject> pool = null;
                GameObject prefab = prefabs[i];
                pool = new ObjectPool<GameObject>
                    (() => CreatePooledItem(prefab, pool), OnTakeFromPool, OnReturnedToPool, OnDestroyOnObject, true);
                gameObjectPools.Add(prefab.name, pool);
            }
        }
    }

    public GameObject Get(string prefabId)
    {
        if (!gameObjectPools.ContainsKey(prefabId))
        {
            CreateAddressableObjectPool(prefabId);
        }
        return gameObjectPools[prefabId].Get();
    }

    public void Clear()
    {
        foreach (var gameObjectPool in gameObjectPools)
        {
            gameObjectPool.Value.Clear();
        }
        gameObjectPools.Clear();
    }

    public void Clear(string prefabId)
    {
        if (gameObjectPools.ContainsKey(prefabId))
        {
            gameObjectPools[prefabId].Clear();
        }
    }

    private void CreateAddressableObjectPool(string prefabId)
    {
        ObjectPool<GameObject> pool = null;
        string referenceName = string.Format(objectPoolIDFormat, prefabId);
        pool = new ObjectPool<GameObject>
            (() => CreatePooledItem(referenceName, pool), OnTakeFromPool, OnReturnedToPool, OnDestroyOnObject, true);
        gameObjectPools.Add(prefabId, pool);
    }

    private GameObject CreatePooledItem(AssetReferenceGameObject reference, IObjectPool<GameObject> pool)
    {
        if (reference.Asset is null)
        {
            var handle = reference.LoadAssetAsync<GameObject>();
            handle.WaitForCompletion();

            if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentException("에셋 로딩 실패");
            }
        }

        return CreatePooledItem(reference.Asset as GameObject, pool);
    }

    private GameObject CreatePooledItem(string key, IObjectPool<GameObject> pool)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("에셋 키 없음");
        }

        var handle = Addressables.InstantiateAsync(key, transform);
        handle.WaitForCompletion();

        if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
        {
            throw new ArgumentException("에셋 로딩 실패");
        }

        GameObject created = handle.Result;
        created.GetComponent<IObjectPoolGameObject>().ObjectPool = pool;
        created.transform.SetParent(transform);
        return created;
    }

    private GameObject CreatePooledItem(GameObject prefab, IObjectPool<GameObject> pool)
    {
        GameObject created = Instantiate(prefab);
        created.GetComponent<IObjectPoolGameObject>().ObjectPool = pool;
        created.transform.SetParent(transform);
        return created;
    }


    private void OnTakeFromPool(GameObject go)
    {
        go.SetActive(true);
    }

    private void OnReturnedToPool(GameObject go)
    {
        go.transform.SetParent(transform);
        go.SetActive(false);
    }

    private void OnDestroyOnObject(GameObject go)
    {
        Destroy(go);
    }
}
