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

    [SerializeField]
    private Transform unit; // TODO: ���� ��Ƽ �Ŵ����κ��� �޾ƿ� �� �ְ� ������ ���� - 250323 HKY

    private void Update()
    {
        if (!isNextStageSpawned && unit.position.z > enterPosition.position.z)
        {
            isNextStageSpawned = true;

            Addressables.InstantiateAsync(stage, stageNextPosition.position, stageNextPosition.rotation).Completed+= (handle)=> handle.Result.GetComponent<Stage>().unit = unit;
        }

        if (unit.position.z > exitPosition.position.z)
        {
            Destroy(gameObject);
        }
    }
}
