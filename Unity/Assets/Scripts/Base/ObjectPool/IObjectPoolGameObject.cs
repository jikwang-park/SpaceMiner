using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IObjectPoolGameObject
{
    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release();
}
