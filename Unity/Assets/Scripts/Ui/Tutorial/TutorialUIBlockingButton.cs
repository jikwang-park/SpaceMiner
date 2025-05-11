using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TutorialUIBlockingButton : MonoBehaviour
{
    [SerializeField]
    private bool isSideMenu = false;

    [SerializeField]
    public int TargetID;

    private int targetPlanet;
    private int targetStage;
    private StageManager stageManager;

    [SerializeField]
    private RectTransform buttonImageRect;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        var stageID = DataTableManager.ContentsOpenTable.GetData(TargetID);
        var stageData = DataTableManager.StageTable.GetData(stageID);
        targetPlanet = stageData.Planet;
        targetStage = stageData.Stage;

        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (targetPlanet < stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage <= stageSaveData.clearedStage))
        {
            gameObject.SetActive(false);
            return;
        }

        stageManager.OnStageEnd += StageManager_OnStageEnd;
    }

    private void StageManager_OnStageEnd()
    {
        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (targetPlanet > stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage > stageSaveData.clearedStage))
        {
            return;
        }
        stageManager.OnStageEnd -= StageManager_OnStageEnd;
        stageManager.StageUiManager.TutorialQueue.EnqueueTutorial(buttonImageRect, isSideMenu);
        gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }
        stageManager.StageUiManager.TutorialQueue.TutorialUIBlocker.Close();
    }
}
