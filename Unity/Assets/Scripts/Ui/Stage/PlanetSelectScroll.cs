using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSelectScroll : MonoBehaviour
{
    [SerializeField]
    private string buttonReference = "PlanetButton";

    [SerializeField]
    private Transform contents;

    public event Action<int> OnPlanetSelected;

    private void Start()
    {
        SetPlanetButtons();
    }

    private void SetPlanetButtons()
    {
        var objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectPoolManager>();
        var planets = DataTableManager.StageTable.GetPlanetKeys();

        for (int i = 0; i < planets.Count; ++i)
        {
            var button = objectpoolManager.gameObjectPool[buttonReference].Get();
            button.transform.SetParent(contents);
            button.transform.localScale = Vector3.one;
            button.GetComponent<PlanetButton>().Set(planets[i]);
            int index = planets[i];
            button.GetComponent<Button>().onClick.AddListener(() => OnPlanetSelected?.Invoke(index));
        }
    }
}
