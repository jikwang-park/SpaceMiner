using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStageUI : MonoBehaviour
{
    private readonly static int[] dungeonTypeExit = new int[] { 154, 156 };


    [SerializeField]
    private LocalizationText tabExitMessage;

    private void OnEnable()
    {
        tabExitMessage.SetString(dungeonTypeExit[Variables.currentDungeonType - 1]);
    }
}
