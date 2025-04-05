using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectWindow : MonoBehaviour
{
    private static readonly int hashSlideIn = Animator.StringToHash("SlideIn");
    private static readonly int hashSlideOut = Animator.StringToHash("SlideOut");

    [SerializeField]
    private PlanetSelectScroll planetScroll;

    [SerializeField]
    private StageSelectScroll stageScroll;

    [SerializeField]
    private Animator animator;

    private int currentPlanet = 1;

    private StageSaveData stageLoadData;

    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Button showButton;

    private void Start()
    {
        planetScroll.OnPlanetSelected += OnPlanetSelected;
        stageLoadData = SaveLoadManager.Data.stageSaveData;
        OnPlanetSelected(stageLoadData.currentPlanet);
        showButton.onClick.AddListener(ShowStageWindow);
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

    private void RefreshStageWindow()
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

    //TODO: 스테이지 선택 버튼 클릭에 연결
    public void ShowStageWindow()
    {
        showButton.onClick.RemoveListener(ShowStageWindow);

        RefreshStageWindow();

        background.SetActive(true);
        animator.SetTrigger(hashSlideIn);

        showButton.onClick.AddListener(HideStageWindow);
    }

    //TODO: 스테이지창 가림 배경에 연결
    public void HideStageWindow()
    {
        showButton.onClick.RemoveListener(HideStageWindow);

        background.SetActive(false);
        animator.SetTrigger(hashSlideOut);

        showButton.onClick.AddListener(ShowStageWindow);
    }
}
