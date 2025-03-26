using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Stage : MonoBehaviour
{
    private int unitCount = 0;
    private bool isNextStageSpawned = false;

    [SerializeField]
    private AssetReferenceGameObject stage;

    [SerializeField]
    private Transform enterPosition;

    [SerializeField]
    private Transform exitPosition;

    [SerializeField]
    private Transform stageNextPosition;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void Update()
    {
        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();
        if (unit is null)
        {
            return;
        }


        if (!isNextStageSpawned && unit.position.z > enterPosition.position.z)
        {
            isNextStageSpawned = true;

            Addressables.InstantiateAsync(stage, stageNextPosition.position, stageNextPosition.rotation);
        }

        if (unit.position.z > exitPosition.position.z)
        {
            Destroy(gameObject);
        }
    }
}
