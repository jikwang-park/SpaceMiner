using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExitConfirmWindow : MonoBehaviour
{
    private readonly static int[] dungeonTypeExitConfirm = new int[] { 155, 157 };

    [SerializeField]
    private LocalizationText exitConfirmWindowMessage;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        exitConfirmWindowMessage.SetString(dungeonTypeExitConfirm[Variables.currentDungeonType - 1]);
    }

    public void OnExitConfirm()
    {
        stageManager.OnExitClicked();
    }
}
