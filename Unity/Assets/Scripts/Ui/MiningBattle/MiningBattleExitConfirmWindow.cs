using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBattleExitWindow : MonoBehaviour
{
    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void OnExitConfirm()
    {
        stageManager.OnExitClicked();
    }
}
