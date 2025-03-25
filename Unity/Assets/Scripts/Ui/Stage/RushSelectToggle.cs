using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RushSelectToggle : MonoBehaviour
{
    private const string Ascend = "Ascend";
    private const string Repeat = "Repeat";

    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        if (Variables.stageMode == StageMode.Repeat)
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void OnRushToggleChanged(bool isOn)
    {
        if (isOn)
        {
            text.text = Ascend;
            Variables.stageMode = StageMode.Ascend;
        }
        else
        {
            text.text = Repeat;
            Variables.stageMode = StageMode.Repeat;
        }
    }
}
