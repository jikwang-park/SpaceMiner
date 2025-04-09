using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

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

    private void OnPlanetSelected(int planet)
    {
        if (currentPlanet == planet)
        {
            return;
        }
        currentPlanet = planet;
        stageScroll.SetButtons(currentPlanet);
    }

    public void RefreshStageWindow()
    {
        var stageData = SaveLoadManager.Data.stageSaveData;
        if (currentPlanet != stageData.currentPlanet)
        {
            OnPlanetSelected(stageData.currentPlanet);
        }
        planetScroll.UnlockPlanet(stageData.highPlanet);

        if (currentPlanet < stageData.highPlanet)
        {
            stageScroll.UnlockStage();
        }
        else
        {
            stageScroll.UnlockStage(stageData.highStage);
        }
    }
}
