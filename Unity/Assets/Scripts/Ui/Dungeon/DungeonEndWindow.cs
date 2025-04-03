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

    public void Set(bool isCleared)
    {
        this.isCleared = isCleared;
    }

    private void OnEnable()
    {
        if (isCleared)
        {
            messageText.text = "Cleared";
            bool lastStageCondition = Variables.currentDungeonStage == DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType);
            var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
            bool keyCondition = ItemManager.GetItemAmount(curStage.DungeonKeyID) >= curStage.KeyCount;

            if (lastStageCondition)
            {
                nextButton.interactable = ItemManager.GetItemAmount(curStage.DungeonKeyID) >= curStage.KeyCount;
                
                nextText.text = "Retry";
            }
            else
            {
                nextText.text = "Next";
                var nextStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage + 1);

                bool powerCondition = Variables.powerLevel > nextStage.ConditionPower;
                bool planetCondition = SaveLoadManager.Data.stageSaveData.highPlanet > nextStage.ConditionPlanet;
                keyCondition = ItemManager.GetItemAmount(nextStage.DungeonKeyID) >= nextStage.KeyCount;
                nextButton.interactable = powerCondition && planetCondition && keyCondition;
            }
        }
        else
        {
            messageText.text = "Failed";
            nextText.text = "Retry";
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
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
    }

    public void Lift()
    {
        ++Variables.currentDungeonStage;
        Retry();
    }

    public void Retry()
    {
        Addressables.LoadSceneAsync("Scenes/DungeonScene").WaitForCompletion();
    }
}
