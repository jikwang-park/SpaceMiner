using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WaveSpawner : MonoBehaviour
{
    [field: SerializeField]
    public Vector3[] SpawnPoints { get; private set; } = new Vector3[]
    {
        new Vector3(3f, 0f, 2f),
        new Vector3(0f, 0f, 2f),
        new Vector3(-3f, 0f, 2f),
        new Vector3(3f, 0f, 6f),
        new Vector3(0f, 0f, 6f),
        new Vector3(-3f, 0f, 6f),
        new Vector3(3f, 0f, 10f),
        new Vector3(0f, 0f, 10f),
        new Vector3(-3f, 0f, 10f),
        new Vector3(3f, 0f, 14f),
        new Vector3(0f, 0f, 14f),
        new Vector3(-3f, 0f, 14f),
    };

    private const string prefabFormat = "Prefabs/Units/{0}";

    public void Spawn(Vector3 frontPosition, CorpsTable.Data data)
    {
        int frontLength = data.NormalMonsterIDs.Length;
        int eachMaxCount = data.FrontSlots / frontLength;
        int[] createdCount = new int[eachMaxCount];
        for (int i = 0; i < data.FrontSlots; ++i)
        {
            int index = Random.Range(0, frontLength);
            if (i < eachMaxCount * frontLength)
            {
                while (createdCount[index] >= eachMaxCount)
                {
                    index = Random.Range(0, frontLength);
                }
            }

            string monsterId = data.NormalMonsterIDs[index];
            Addressables.InstantiateAsync(string.Format(prefabFormat, monsterId),
                frontPosition + SpawnPoints[i],
                Quaternion.LookRotation(Vector3.back, Vector3.up));
            ++createdCount[index];
        }

        int backStartPos = Mathf.CeilToInt((float)data.FrontSlots / 3) * 3;

        for (int i = 0, j = backStartPos; i < data.BackSlots; ++i, ++j)
        {
            Addressables.InstantiateAsync(string.Format(prefabFormat, data.RangedMonsterID),
                frontPosition + SpawnPoints[j], 
                Quaternion.LookRotation(Vector3.back, Vector3.up));
        }
    }
}
