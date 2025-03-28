using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject[] addressableAssets;

    [SerializeField]
    private GameObject[] prefabs;

    public Dictionary<string, IObjectPool<GameObject>> gameObjectPool { get; private set; } = new Dictionary<string, IObjectPool<GameObject>>();

    private void Awake()
    {
        for (int i = 0; i < addressableAssets.Length; ++i)
        {
            var handle = addressableAssets[i].LoadAssetAsync<GameObject>();
            handle.WaitForCompletion();

            if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new ArgumentException("에셋 로딩 실패");
            }

            if (!gameObjectPool.ContainsKey(handle.Result.name))
            {
                ObjectPool<GameObject> pool = null;
                AssetReferenceGameObject reference = addressableAssets[i];
                pool = new ObjectPool<GameObject>
                    (() => CreatePooledItem(reference.Asset as GameObject, pool), OnTakeFromPool, OnReturnedToPool, OnDestroyOnObject, true);
                gameObjectPool.Add(handle.Result.name, pool);
            }
        }
        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (!gameObjectPool.ContainsKey(prefabs[i].name))
            {
                ObjectPool<GameObject> pool = null;
                GameObject prefab = prefabs[i];
                pool = new ObjectPool<GameObject>
                    (() => CreatePooledItem(prefab, pool), OnTakeFromPool, OnReturnedToPool, OnDestroyOnObject, true);
                gameObjectPool.Add(prefab.name, pool);
            }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < addressableAssets.Length; ++i)
        {
            if (addressableAssets[i].Asset is not null)
            {
                addressableAssets[i].ReleaseAsset();
            }
        }
    }

    private GameObject CreatePooledItem(AssetReferenceGameObject reference, IObjectPool<GameObject> pool)
    {
        return CreatePooledItem(reference.Asset as GameObject, pool);
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
