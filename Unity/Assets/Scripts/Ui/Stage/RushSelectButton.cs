using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RushSelectButton : MonoBehaviour
{
    private const string Ascend = "Ascend";
    private const string Repeat = "Repeat";

    [SerializeField]
    private TextMeshProUGUI text;

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
