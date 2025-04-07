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

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Button showButton;

    private bool isShown = false;

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

    //TODO: 스테이지 선택 버튼에 연결
    public void ToggleStageWindow()
    {
        if (isShown)
        {
            HideStageWindow();
        }
        else
        {
            isShown = true;
            background.SetActive(true);
        }
    }

    public void HideStageWindow()
    {
        isShown = false;
        background.SetActive(false);
    }
}
