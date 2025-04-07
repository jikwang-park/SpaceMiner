using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(28f, 20f, -23f);

    [SerializeField]
    private Transform unit;

    [SerializeField]
    private Transform worldCamera;

    [SerializeField]
    private float followingSpeed = 3f;

    private UnitPartyManager unitPartyManager;

    private void Start()
    {
        unitPartyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitPartyManager>();
    }

    private void Update()
    {
        unit = unitPartyManager.GetFirstLineUnitTransform();
        if (unit is null)
        {
            return;
        }
        if (Vector3.SqrMagnitude(unit.position + offset - worldCamera.position) < 0.001f)
        {
            return;
        }
        worldCamera.position = Vector3.Lerp(worldCamera.position, unit.position + offset, Time.deltaTime * followingSpeed);
    }

    public void ResetCameraPosition()
    {
        unit = unitPartyManager.GetFirstLineUnitTransform();
        if (unit is null)
        {
            worldCamera.position = offset;
            return;
        }
        worldCamera.position = unit.position + offset;
    }
}
