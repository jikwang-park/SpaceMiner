using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RushSelectToggle : MonoBehaviour
{
    private const int Ascend = 45;
    private const int Repeat = 46;

    [SerializeField]
    private LocalizationText text;

    private StageEndWindow stageEndWindow;

    private void Start()
    {
        if (Variables.stageMode == StageMode.Repeat)
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void OnRushToggleChanged(bool isOn)
    {
        if (stageEndWindow is null)
        {
            stageEndWindow = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().StageUiManager.StageEndWindow;
        }

        if (isOn)
        {
            text.SetString(Ascend);
            Variables.stageMode = StageMode.Ascend;
            stageEndWindow.ShowPlanetStageMode(true);
        }
        else
        {
            text.SetString(Repeat);
            Variables.stageMode = StageMode.Repeat;
            if (!stageEndWindow.isShowing)
            {
                stageEndWindow.ShowPlanetStageMode(false);
            }
        }
    }
}
