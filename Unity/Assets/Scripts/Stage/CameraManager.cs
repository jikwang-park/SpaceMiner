using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 Offset = new Vector3(15f, 10f, -11f);

    [SerializeField]
    private Transform Unit;

    [SerializeField]
    private float followingSpeed = 3f;

    private UnitPartyManager unitPartyManager;

    private void Start()
    {
        unitPartyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitPartyManager>();
    }

    private void Update()
    {
        Unit = unitPartyManager.GetFirstLineUnit();
        if (Unit != null)
        {
            transform.position = Vector3.Lerp(transform.position, Unit.position + Offset, Time.deltaTime * followingSpeed);
        }
    }
}
