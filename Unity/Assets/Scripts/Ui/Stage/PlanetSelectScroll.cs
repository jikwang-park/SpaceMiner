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
    private List<PlanetButton> buttons = new List<PlanetButton>();

    private void Start()
    {
        SetPlanetButtons();
    }

    private void SetPlanetButtons()
    {
        var objectpoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().stageUiManager.GetComponent<ObjectPoolManager>();
        var planets = DataTableManager.StageTable.GetPlanetKeys();

        for (int i = 0; i < planets.Count; ++i)
        {
            var button = objectpoolManager.gameObjectPool[buttonReference].Get();
            button.transform.SetParent(contents);
            button.transform.localScale = Vector3.one;

            var planetButton = button.GetComponent<PlanetButton>();
            planetButton.Set(planets[i]);
            buttons.Add(planetButton);

            int index = planets[i];
            planetButton.Button.onClick.AddListener(() => OnPlanetSelected?.Invoke(index));
            if (i < Variables.maxPlanetNumber)
            {
                planetButton.Button.interactable = true;
            }
        }
    }

    public void UnlockPlanet(int planet)
    {
        if (buttons.Count < planet)
        {
            return;
        }

        buttons[planet - 1].Button.interactable = true;
    }
}
