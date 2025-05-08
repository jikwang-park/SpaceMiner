using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBattleResultWindow : MonoBehaviour
{
    [SerializeField]
    private LocalizationText stageText;

    [SerializeField]
    private GameObject clearView;

    [SerializeField]
    private GameObject defeatView;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void ShowClear(MiningBattleTable.Data data, List<(int itemid, BigNumber amount)> gainItem)
    {
        stageText.SetString(60011, data.Stage.ToString());
        gameObject.SetActive(true);
        clearView.SetActive(true);
        defeatView.SetActive(false);

    }

    public void ShowDefeat(MiningBattleTable.Data data)
    {
        gameObject.SetActive(true);
        defeatView.SetActive(true);
        clearView.SetActive(false);
    }

    public void NextStage()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            ++Variables.planetMiningStage;
            stageManager.MiningBattleStart();
        }
    }
}
