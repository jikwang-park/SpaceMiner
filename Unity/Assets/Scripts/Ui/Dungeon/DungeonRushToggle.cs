using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonRushToggle : MonoBehaviour
{
    private const string Ascend = "돌파";
    private const string End = "돌파 안함";

    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        if (Variables.dungeonMode == DungeonMode.End)
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

    public void OnRushToggleChanged(bool isOn)
    {
        if (isOn)
        {
            text.text = Ascend;
            Variables.dungeonMode = DungeonMode.Ascend;
        }
        else
        {
            text.text = End;
            Variables.dungeonMode = DungeonMode.End;
        }
    }
}
