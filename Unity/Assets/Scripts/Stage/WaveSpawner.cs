using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WaveSpawner : MonoBehaviour
{
    [field: SerializeField]
    public Vector3[] SpawnPoints { get; private set; } = new Vector3[]
    {
        new Vector3(-3f, 0f, 2f),
        new Vector3(0f, 0f, 2f),
        new Vector3(3f, 0f, 2f),
        new Vector3(-3f, 0f, 6f),
        new Vector3(0f, 0f, 6f),
        new Vector3(3f, 0f, 6f),
        new Vector3(-3f, 0f, 10f),
        new Vector3(0f, 0f, 10f),
        new Vector3(3f, 0f, 10f),
        new Vector3(-3f, 0f, 14f),
        new Vector3(0f, 0f, 14f),
        new Vector3(3f, 0f, 14f),
    };

    public Vector3[] SpawnOffsets { get; private set; } = new Vector3[]
    {
        Vector3.back,
        Vector3.forward,
    };

    private const string prefabFormat = "Prefabs/Units/{0}";

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GetComponent<StageManager>();
    }

    public void Spawn(Vector3 frontPosition, CorpsTable.Data data)
    {
        if (data.FrontSlots == 0 && data.BackSlots == 0 && data.BossMonsterID != "0")
        {
            int lane = 1;
            var handle = Addressables.InstantiateAsync(string.Format(prefabFormat, data.BossMonsterID),
                frontPosition + SpawnPoints[lane],
                Quaternion.LookRotation(Vector3.back, Vector3.up));
            handle.Completed += (eventHandle) => SetMonsterLane(lane, eventHandle);
            return;
        }

        int frontTypeCount = data.NormalMonsterIDs.Length;
        int eachMaxCount = data.FrontSlots / frontTypeCount;
        int[] createdCount = new int[eachMaxCount];

        for (int i = 0; i < data.FrontSlots; ++i)
        {
            int index = Random.Range(0, frontTypeCount);
            if (i < eachMaxCount * frontTypeCount)
            {
                while (createdCount[index] >= eachMaxCount)
                {
                    index = Random.Range(0, frontTypeCount);
                }
            }

            string monsterId = data.NormalMonsterIDs[index];
            int lane = i % 3;
            var handle = Addressables.InstantiateAsync(string.Format(prefabFormat, monsterId),
                frontPosition + SpawnPoints[i] + SpawnOffsets[0],
                Quaternion.LookRotation(Vector3.back, Vector3.up));
            handle.WaitForCompletion();
            SetMonsterLane(lane, handle);
            var handle2 = Addressables.InstantiateAsync(string.Format(prefabFormat, monsterId),
                frontPosition + SpawnPoints[i] + SpawnOffsets[1],
                Quaternion.LookRotation(Vector3.back, Vector3.up));
            handle2.WaitForCompletion();
            SetMonsterLane(lane, handle2);

            ++createdCount[index];
        }

        int backStartPos = Mathf.CeilToInt((float)data.FrontSlots / 3) * 3;

        for (int i = 0, j = backStartPos; i < data.BackSlots; ++i, ++j)
        {
            var handle = Addressables.InstantiateAsync(string.Format(prefabFormat, data.RangedMonsterID),
                frontPosition + SpawnPoints[j],
            Quaternion.LookRotation(Vector3.back, Vector3.up));
            int lane = i % 3;

            handle.Completed += (eventHandle) => SetMonsterLane(lane, eventHandle);
        }
    }

    private void SetMonsterLane(int lane, AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            return;
        }

        var monsterController = handle.Result.GetComponent<MonsterController>();
        if (monsterController == null)
        {
            return;
        }
        stageManager.AddMonster(monsterController);
        stageManager.MonsterLaneManager.AddMonster(lane, monsterController);
    }
}
