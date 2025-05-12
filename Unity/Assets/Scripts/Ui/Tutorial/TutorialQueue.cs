using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TutorialQueue : MonoBehaviour
{
    [Serializable]
    public class TutorialList
    {
        public bool isSideMenu;
        public int targetID;
        public RectTransform buttonImageRect;
        public bool isShown;
    }

    [SerializeField]
    private RectTransform sideMenuOpenButton;

    [SerializeField]
    private GameObject maximizedSidebar;

    private Queue<(RectTransform rect, bool isSideBar)> queue = new Queue<(RectTransform rect, bool isSideBar)>();

    [field: SerializeField]
    public TutorialUIBlocker TutorialUIBlocker { get; private set; }

    [SerializeField]
    private TutorialPage tutorialPage;

    [SerializeField]
    private TutorialList[] unlocks;

    private StageManager stageManager;

    private bool maximize = false;


    private void Update()
    {
        if (tutorialPage.gameObject.activeInHierarchy)
        {
            return;
        }
        if (queue.Count > 0)
        {
            if (queue.Peek().isSideBar)
            {
                if (!maximizedSidebar.activeInHierarchy && !TutorialUIBlocker.isShowing)
                {
                    TutorialUIBlocker.SetBlock(sideMenuOpenButton);
                    TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Down);
                    maximize = true;
                }
                if (maximizedSidebar.activeInHierarchy
                    && ((TutorialUIBlocker.isShowing && maximize) || !TutorialUIBlocker.isShowing))
                {
                    maximize = false;

                    TutorialUIBlocker.Close();
                    TutorialUIBlocker.SetBlock(queue.Dequeue().rect);
                    TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Down);
                }
            }
            else if (!TutorialUIBlocker.isShowing)
            {
                TutorialUIBlocker.SetBlock(queue.Dequeue().rect);
                TutorialUIBlocker.ShowArrow(TutorialUIBlocker.Position.Up);
            }
        }
    }

    public void EnqueueTutorial(RectTransform rectTransform, bool isSideBar)
    {
        queue.Enqueue((rectTransform, isSideBar));
    }

    // 사이드 메뉴 한번도 안연 경우 제대로 표시되지 않는 버그로 인해 매니저에 포함

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        var stageSaveData = SaveLoadManager.Data.stageSaveData;

        int shownCount = 0;
        for (int i = 0; i < unlocks.Length; ++i)
        {
            var stageID = DataTableManager.ContentsOpenTable.GetData(unlocks[i].targetID);
            var stageData = DataTableManager.StageTable.GetData(stageID);
            int targetPlanet = stageData.Planet;
            int targetStage = stageData.Stage;
            unlocks[i].isShown = targetPlanet < stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage <= stageSaveData.clearedStage);
            if (unlocks[i].isShown)
            {
                ++shownCount;
            }
        }
        if (shownCount < unlocks.Length)
        {
            stageManager.OnStageEnd += StageManager_OnStageEnd;
        }
        else
        {
            enabled = false;
        }
    }

    private void StageManager_OnStageEnd()
    {
        var stageSaveData = SaveLoadManager.Data.stageSaveData;

        int shownCount = 0;

        for (int i = 0; i < unlocks.Length; ++i)
        {
            if (unlocks[i].isShown)
            {
                ++shownCount;
                continue;
            }

            var stageID = DataTableManager.ContentsOpenTable.GetData(unlocks[i].targetID);
            var stageData = DataTableManager.StageTable.GetData(stageID);
            int targetPlanet = stageData.Planet;
            int targetStage = stageData.Stage;

            if (targetPlanet > stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage > stageSaveData.clearedStage))
            {
                continue;
            }
            ++shownCount;
            unlocks[i].isShown = true;
            stageManager.StageUiManager.TutorialQueue.EnqueueTutorial(unlocks[i].buttonImageRect, unlocks[i].isSideMenu);
        }

        if (shownCount == unlocks.Length)
        {
            stageManager.OnStageEnd -= StageManager_OnStageEnd;
        }
    }

    public void CloseUIBlocker()
    {
        TutorialUIBlocker.Close();
        if (queue.Count > 0)
        {
            return;
        }

        int shownCount = 0;

        for (int i = 0; i < unlocks.Length; ++i)
        {
            if (unlocks[i].isShown)
            {
                ++shownCount;
            }
        }

        if (shownCount == unlocks.Length)
        {
            enabled = false;
        }
    }
}
