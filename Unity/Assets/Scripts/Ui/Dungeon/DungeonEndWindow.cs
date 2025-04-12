using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonEndWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nextText;
    [SerializeField]
    private TextMeshProUGUI messageText;

    private float closeTime;
    private WaitForSeconds wait = new WaitForSeconds(1f);

    [SerializeField]
    private Button nextButton;
    private bool isCleared;

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void Open(string message, bool isCleared)
    {
        this.isCleared = isCleared;

        gameObject.SetActive(true);

        if (this.isCleared)
        {
            messageText.text = "Cleared";
            bool lastStageCondition = Variables.currentDungeonStage == DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType);
            var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
            bool keyCondition = ItemManager.GetItemAmount(curStage.NeedKeyItemID) >= curStage.NeedKeyCount;

            if (lastStageCondition)
            {
                nextButton.interactable = ItemManager.GetItemAmount(curStage.NeedKeyItemID) >= curStage.NeedKeyCount;

                nextText.text = "Retry";
            }
            else
            {
                nextText.text = "Next";
                var nextStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage + 1);

                bool powerCondition = Variables.powerLevel > nextStage.NeedPower;
                bool planetCondition = (SaveLoadManager.Data.stageSaveData.highPlanet > nextStage.NeedClearPlanet)
                    || (SaveLoadManager.Data.stageSaveData.highPlanet == SaveLoadManager.Data.stageSaveData.clearedPlanet
                        && SaveLoadManager.Data.stageSaveData.highStage == SaveLoadManager.Data.stageSaveData.clearedStage);
                keyCondition = ItemManager.GetItemAmount(nextStage.NeedKeyItemID) >= nextStage.NeedKeyCount;
                nextButton.interactable = powerCondition && planetCondition && keyCondition;
            }
        }
        else
        {
            messageText.text = "Failed";
            nextText.text = "Retry";
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        stageManager.SetStatus(IngameStatus.Planet);

        gameObject.SetActive(false);
    }

    public void RightButton()
    {
        if (isCleared
            && Variables.currentDungeonStage < DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType))
        {
            Lift();
        }
        else
        {
            Retry();
        }

        gameObject.SetActive(false);
    }

    public void Lift()
    {
        ++Variables.currentDungeonStage;
        Retry();
    }

    public void Retry()
    {
        stageManager.ResetStage();
    }
}
