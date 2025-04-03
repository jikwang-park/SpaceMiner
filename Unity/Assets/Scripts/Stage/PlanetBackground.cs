using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

public class PlanetBackground : MonoBehaviour, IObjectPoolGameObject
{
    private bool isNextStageSpawned = false;

    [SerializeField]
    private Transform enterPosition;

    [SerializeField]
    private Transform exitPosition;

    [SerializeField]
    private Transform stageNextPosition;

    private StageManager stageManager;

    public IObjectPool<GameObject> ObjectPool { get; set; }

    public void Release()
    {
        isNextStageSpawned = false;
        ObjectPool.Release(gameObject);
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void Update()
    {
        if(stageManager.UnitPartyManager.UnitCount ==0)
        {
            return;
        }

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();

        if (!isNextStageSpawned && unit.position.z > enterPosition.position.z)
        {
            isNextStageSpawned = true;

            var nextBackground = ObjectPool.Get();
            nextBackground.transform.parent = null;
            nextBackground.transform.position = stageNextPosition.position;
            nextBackground.transform.rotation = stageNextPosition.rotation;
        }

        if (unit.position.z > exitPosition.position.z)
        {
            Release();
        }
    }
}
