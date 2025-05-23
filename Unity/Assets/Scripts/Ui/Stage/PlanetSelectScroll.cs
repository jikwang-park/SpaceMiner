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
    private ObjectPoolManager objectPoolManager;

#if UNITY_EDITOR
    private bool debugMode = false;
#endif

    private void Start()
    {
        SetPlanetButtons();
    }

    private void SetPlanetButtons()
    {
        objectPoolManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().StageUiManager.ObjectPoolManager;
        var planets = DataTableManager.StageTable.GetPlanetKeys();

        for (int i = 0; i < planets.Count; ++i)
        {
            var button = objectPoolManager.Get(buttonReference);
            button.transform.SetParent(contents);
            button.transform.localScale = Vector3.one;

            var planetButton = button.GetComponent<PlanetButton>();
            planetButton.Set(planets[i]);
            buttons.Add(planetButton);

            int index = planets[i];
            planetButton.Button.onClick.AddListener(() => OnPlanetSelected?.Invoke(index));
            if (i < SaveLoadManager.Data.stageSaveData.highPlanet)
            {
                planetButton.Button.interactable = true;
            }
#if UNITY_EDITOR
            else if (debugMode)
            {
                planetButton.Button.interactable = true;
            }
#endif
        }
    }


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            debugMode = !debugMode;
            foreach (var button in buttons)
            {
                button.Button.interactable = true;
            }
        }
    }
#endif

    public void UnlockPlanet(int planet)
    {
        if (buttons.Count == 0)
        {
            return;
        }

        for (int i = 0; i < planet; ++i)
        {
            buttons[i].Button.interactable = true;
        }
    }
}
