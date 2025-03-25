using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSelectScroll : MonoBehaviour
{
    [SerializeField]
    private string buttonReference = "PlanetButton";

    [SerializeField]
    private Transform contents;

    public event Action<int> OnPlanetSelected;

    private void Start()
    {
        var objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPoolManager>();
        var button = objectpoolManager.gameObjectPool[buttonReference].Get();
        button.transform.SetParent(contents);
        button.transform.localScale = Vector3.one;
        button.GetComponent<PlanetButton>().Set(1);
    }

}
