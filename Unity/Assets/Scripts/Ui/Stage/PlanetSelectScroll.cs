using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectScroll : MonoBehaviour
{
    [SerializeField]
    private string buttonReference = "StageButton";

    [SerializeField]
    private Transform contents;

    private void Start()
    {
        var objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPoolManager>();
        var button = objectpoolManager.gameObjectPool[buttonReference].Get();
        button.transform.parent = contents;
        button.transform.localScale = Vector3.one;
        button.GetComponent<StageButton>().Set(1, 1);
    }
}
