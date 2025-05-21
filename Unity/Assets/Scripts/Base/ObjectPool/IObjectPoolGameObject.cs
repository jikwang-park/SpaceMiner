using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IObjectPoolGameObject
{
    IObjectPool<GameObject> ObjectPool { get; set; }

    void Release();
}
