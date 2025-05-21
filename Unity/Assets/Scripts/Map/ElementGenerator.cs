using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject leftSide;
    [SerializeField]
    private GameObject rightSide;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;
  

    [SerializeField]
    private List<GameObject> smallPrefab;
    [SerializeField]
    private List<GameObject> largePrefab;

    [SerializeField]
    public float largeObjectChance = 0.2f; 


    [SerializeField]
    private float leftZoneMinX = -30f;  
    [SerializeField]
    private float leftZoneMaxX = -10f;
    [SerializeField]
    private float rightZoneMinX = 5f;
    [SerializeField]
    private float rightZoneMaxX = 30f;

    [SerializeField]
    private const int maxAttempts = 10;
    [SerializeField]
    private const float minScale = 3f;
    [SerializeField]
    private const float maxScale = 5f;
    [SerializeField]
    private LayerMask obstacleMask;
    private void SpawnLargeElement()
    {

        for(int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float minZ = Mathf.Min(startPos.position.z, endPos.position.z);
            float maxZ = Mathf.Max(startPos.position.z, endPos.position.z);
            float z = Random.Range(minZ, maxZ);

            float largePrefabXPos = Random.Range(leftZoneMaxX, leftZoneMaxX);
            Vector3 spawnPos = new Vector3(largePrefabXPos, 0f, z);

            GameObject prefab = largePrefab[Random.Range(0, largePrefab.Count)];
            float scale = Random.Range(minScale, maxScale);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Vector3 prefabSize = prefab.GetComponent<Renderer>().bounds.size;
            Vector3 scaledSize = prefabSize * scale;
            Vector3 halfExtents = scaledSize / 2f;

            Vector3 boxCenter = spawnPos + Vector3.up * 0.5f;
            Vector3 boxHalfExtents = new Vector3(halfExtents.x, 0.5f, halfExtents.z);

            bool isAreaFree = Physics.OverlapBox(boxCenter, boxHalfExtents, rotation, obstacleMask).Length == 0;

        }
    }

    private bool IsAreaFree(Vector3 center, Vector3 halfExtents)
    {
        Collider[] hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity);
        return hits.Length == 0;
    }


    private void SpawnElement()
    {
        float leftSideX = Random.Range(leftZoneMinX, leftZoneMaxX);
        float rightSIdeX = Random.Range(rightZoneMinX, rightZoneMaxX);
        float minZ = Mathf.Min(startPos.position.z, endPos.position.z);
        float maxZ = Mathf.Max(startPos.position.z, endPos.position.z);
        float z = Random.Range(minZ, maxZ);

        bool spawnOnLeft = Random.value < 0.5f;

        float x = spawnOnLeft ? leftSideX : rightSIdeX;
    }


}
