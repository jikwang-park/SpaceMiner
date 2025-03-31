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

    private StageManager stageManager;
    private ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        stageManager = GetComponent<StageManager>();
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }

    public void Spawn(Vector3 frontPosition, CorpsTable.Data data)
    {
        if (data.FrontSlots == 0 && data.BackSlots == 0 && data.BossMonsterID != 0)
        {
            int lane = 1;
            SpawnMonster(lane, lane, frontPosition, data.BossMonsterID);
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

            int monsterId = data.NormalMonsterIDs[index];
            int lane = i % 3;
            SpawnMonster(lane, i, frontPosition, monsterId);
            ++createdCount[index];
        }

        int backStartPos = Mathf.CeilToInt((float)data.FrontSlots / 3) * 3;

        for (int i = 0, j = backStartPos; i < data.BackSlots; ++i, ++j)
        {
            int lane = i % 3;
            SpawnMonster(lane, j, frontPosition, data.RangedMonsterID);
        }
    }

    private void SetMonsterLane(int lane, GameObject gameObject)
    {
        var monsterController = gameObject.GetComponent<MonsterController>();
        if (monsterController is null)
        {
            return;
        }
        stageManager.AddMonster(monsterController);
        stageManager.MonsterLaneManager.AddMonster(lane, monsterController);
    }

    private void SpawnMonster(int lane, int index, Vector3 frontPosition, int monsterId)
    {
        var monsterData =  DataTableManager.MonsterTable.GetData(monsterId);

        var monster = objectPoolManager.gameObjectPool[monsterData.PrefabId].Get();
        var monsterController = monster.GetComponent<MonsterController>();
        monsterController.SetMonsterId(monsterId);
        monster.transform.parent = null;
        monster.transform.position = frontPosition + SpawnPoints[index];
        monster.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        SetMonsterLane(lane, monster);
    }
}
