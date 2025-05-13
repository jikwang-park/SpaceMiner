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

    private MessageWindow messageWindow;

    private void Start()
    {
        if (Variables.stageMode == StageMode.Repeat)
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void OnRushToggleChanged(bool isOn)
    {
        if (messageWindow is null)
        {
            messageWindow = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>().StageUiManager.MessageWindow;
        }

        if (isOn)
        {
            text.SetString(Ascend);
            Variables.stageMode = StageMode.Ascend;
            messageWindow.ShowPlanetStageMode(true);
        }
        else
        {
            text.SetString(Repeat);
            Variables.stageMode = StageMode.Repeat;
            if (!messageWindow.isShowing)
            {
                messageWindow.ShowPlanetStageMode(false);
            }
        }
    }
}
