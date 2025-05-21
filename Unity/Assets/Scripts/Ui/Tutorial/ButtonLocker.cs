using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLocker : MonoBehaviour
{
    private static readonly Color grey = new Color(0.60f, 0.60f, 0.60f);

    [SerializeField]
    public int TargetID;

    [SerializeField]
    public Image buttonImage;

    private int targetPlanet;
    private int targetStage;
    private StageManager stageManager;

    [SerializeField]
    private bool shouldButtonImageHide;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();

        var stageID = DataTableManager.ContentsOpenTable.GetData(TargetID);
        var stageData = DataTableManager.StageTable.GetData(stageID);
        targetPlanet = stageData.Planet;
        targetStage = stageData.Stage;

        Check();

        var stageSaveData = SaveLoadManager.Data.stageSaveData;

        if (targetPlanet < stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage <= stageSaveData.clearedStage))
        {
            return;
        }

        stageManager.OnStageEnd += StageManager_OnStageEnd;
    }

    private void StageManager_OnStageEnd()
    {
        Check();
    }

    private void Check()
    {
        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (targetPlanet > stageSaveData.clearedPlanet
            || (targetPlanet == stageSaveData.clearedPlanet && targetStage > stageSaveData.clearedStage))
        {
            ChangeImage(false);
            return;
        }
        stageManager.OnStageEnd -= StageManager_OnStageEnd;
        ChangeImage(true);
        gameObject.SetActive(false);
    }

    private void ChangeImage(bool isOn)
    {
        if (buttonImage is null)
        {
            return;
        }
        if (isOn)
        {
            buttonImage.enabled = true;
            buttonImage.color = Color.white;
        }
        else if (shouldButtonImageHide)
        {
            buttonImage.enabled = false;
        }
        else
        {
            buttonImage.color = grey;
        }
    }

    public void OnClickButton()
    {
        stageManager.StageUiManager.MessageWindow.ShowStageRestrict(targetPlanet, targetStage);
    }
}
