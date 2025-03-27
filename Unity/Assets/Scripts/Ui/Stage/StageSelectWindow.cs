using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectWindow : MonoBehaviour
{
    [SerializeField]
    private PlanetSelectScroll planetScroll;

    [SerializeField]
    private StageSelectScroll stageScroll;

    private int currentPlanet = 1;

    private void Start()
    {
        planetScroll.OnPlanetSelected += OnPlanetSelected;
    }

    public void UnlockStage(int planet, int stage)
    {
        planetScroll.UnlockPlanet(planet);
        stageScroll.UnlockStage(stage);
    }

    private void OnPlanetSelected(int planet)
    {
        if (currentPlanet == planet)
        {
            return;
        }
        currentPlanet = planet;
        stageScroll.SetButtons(currentPlanet);
    }
}
