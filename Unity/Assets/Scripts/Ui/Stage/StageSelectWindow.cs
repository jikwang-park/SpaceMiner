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

    private StageSaveData stageLoadData;


    private void Start()
    {
        planetScroll.OnPlanetSelected += OnPlanetSelected;
        stageLoadData = SaveLoadManager.Data.stageSaveData;
        OnPlanetSelected(stageLoadData.currentPlanet);
    }

    public void UnlockStage(int planet, int stage)
    {
        if(currentPlanet != planet)
        {
            OnPlanetSelected(planet);
        }
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
