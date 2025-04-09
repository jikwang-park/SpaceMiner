using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 defaultOffset = new Vector3(28f, 20f, -23f);

    [SerializeField]
    private Vector3 defaultRotation = new Vector3(35f, -45f, 0f);

    [SerializeField]
    private Transform unit;

    [SerializeField]
    private Transform worldCamera;

    [SerializeField]
    private float followingSpeed = 3f;

    private UnitPartyManager unitPartyManager;

    public Vector3 Offset { get; private set; }
    public Vector3 Rotation { get; private set; }

    private void Start()
    {
        unitPartyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitPartyManager>();
        Offset = defaultOffset;
        Rotation = defaultRotation;
    }

    private void Update()
    {
        unit = unitPartyManager.GetFirstLineUnitTransform();
        if (unit is null)
        {
            return;
        }
        if (Vector3.SqrMagnitude(unit.position + Offset - worldCamera.position) < 0.001f)
        {
            return;
        }
        worldCamera.position = Vector3.Lerp(worldCamera.position, unit.position + Offset, Time.deltaTime * followingSpeed);
    }

    public void SetCameraOffset()
    {
        SetCameraOffset(defaultOffset);
    }

    public void SetCameraOffset(Vector3 offset)
    {
        Offset = offset;
        if (unitPartyManager.UnitCount > 0)
        {
            unit = unitPartyManager.GetFirstLineUnitTransform();
            worldCamera.position = unit.position + Offset;
        }
        else
        {
            unit = null;
            worldCamera.position = Offset;
        }
    }

    public void SetCameraRotation()
    {
        SetCameraRotation(defaultRotation);
    }

    public void SetCameraRotation(Vector3 euler)
    {
        Rotation = euler;
        worldCamera.rotation = Quaternion.Euler(euler);
    }
}
