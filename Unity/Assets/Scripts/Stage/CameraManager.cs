using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 Offset = new Vector3(15f, 15f, -10f);

    [SerializeField]
    private Transform Unit;

    [SerializeField]
    private float followingSpeed = 3f;

    void Update()
    {
        if (Unit != null)
            transform.position = Vector3.Lerp(transform.position, Unit.position + Offset, Time.deltaTime * followingSpeed);
    }
}
