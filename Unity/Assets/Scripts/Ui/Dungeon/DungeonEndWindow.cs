using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonEndWindow : MonoBehaviour
{
    private const int ClearID = 65;
    private const int FailID = 65;
    private const int NextID = 45;
    private const int RetryID = 46;

    [SerializeField]
    private LocalizationText nextText;
    [SerializeField]
    private LocalizationText messageText;
    [SerializeField]
    private AddressableImage icon;
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private GameObject rewardRow;

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

    public void Open(bool isCleared, bool firstCleared)
    {
        this.isCleared = isCleared;

        gameObject.SetActive(true);
        rewardRow.SetActive(this.isCleared);
        if (this.isCleared)
        {
            messageText.SetColor(Color.white);
            messageText.SetString(ClearID);
            bool lastStageCondition = Variables.currentDungeonStage == DataTableManager.DungeonTable.CountOfStage(Variables.currentDungeonType);
            var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
            bool keyCondition = ItemManager.GetItemAmount(curStage.NeedKeyItemID) >= curStage.NeedKeyItemCount;

            var itemData = DataTableManager.ItemTable.GetData(curStage.RewardItemID);
            icon.SetSprite(itemData.SpriteID);
            if (firstCleared)
            {
                countText.text = curStage.FirstClearRewardItemCount.ToString();
            }
            else
            {
                countText.text = curStage.ClearRewardItemCount.ToString();
            }

            if (lastStageCondition)
            {
                nextButton.interactable = ItemManager.GetItemAmount(curStage.NeedKeyItemID) >= curStage.NeedKeyItemCount;

                nextText.SetString(RetryID);
            }
            else
            {
                nextText.SetString(NextID);
                var nextStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage + 1);

                bool powerCondition = Variables.powerLevel > nextStage.NeedPower;
                bool planetCondition = (SaveLoadManager.Data.stageSaveData.highPlanet > nextStage.NeedClearPlanet)
                    || (SaveLoadManager.Data.stageSaveData.highPlanet == SaveLoadManager.Data.stageSaveData.clearedPlanet
                        && SaveLoadManager.Data.stageSaveData.highStage == SaveLoadManager.Data.stageSaveData.clearedStage);
                keyCondition = ItemManager.GetItemAmount(nextStage.NeedKeyItemID) >= nextStage.NeedKeyItemCount;
                nextButton.interactable = powerCondition && planetCondition && keyCondition;
            }
        }
        else
        {
            messageText.SetColor(Color.red);
            messageText.SetString(FailID);
            nextText.SetString(RetryID);
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
