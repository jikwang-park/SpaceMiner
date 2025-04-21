using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEndWindow : MonoBehaviour
{
    private const int ClearID = 142;
    private const int FailID = 143;

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
    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    private float closeTime;

    [SerializeField]
    private GameObject twoButtons;
    [SerializeField]
    private GameObject threeButtons;

    private bool isCleared;

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void Open(BigNumber damage)
    {
        gameObject.SetActive(true);
        messageText.SetString(60011, damage.ToString());

        var rewardData = DataTableManager.DamageDungeonRewardTable.GetData(damage);
        if (rewardData is not null)
        {
            rewardRow.SetActive(true);

            var itemData = DataTableManager.ItemTable.GetData(rewardData.RewardItemID);
            icon.SetSprite(itemData.SpriteID);
            countText.text = rewardData.RewardItemCount.ToString();
        }

        twoButtons.SetActive(true);
        threeButtons.SetActive(false);
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
                twoButtons.SetActive(true);
                threeButtons.SetActive(false);
            }
            else
            {
                twoButtons.SetActive(false);
                threeButtons.SetActive(true);
            }
        }
        else
        {
            messageText.SetColor(Color.red);
            messageText.SetString(FailID);
            twoButtons.SetActive(true);
            threeButtons.SetActive(false);
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

    public void Lift()
    {
        var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (ItemManager.GetItemAmount(curStage.NeedKeyItemID) < curStage.NeedKeyItemCount)
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
            return;
        }

        var nextStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage + 1);
        if ((stageSaveData.highPlanet < nextStage.NeedClearPlanet)
            || (stageSaveData.highPlanet == nextStage.NeedClearPlanet
                && stageSaveData.highStage != stageSaveData.clearedStage))
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.StageClear);
            return;
        }

        if (UnitCombatPowerCalculator.ToTalCombatPower < nextStage.NeedPower)
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.Power);
            return;
        }

        ++Variables.currentDungeonStage;
        stageManager.ResetStage();
        gameObject.SetActive(false);
    }

    public void Retry()
    {
        var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (ItemManager.GetItemAmount(curStage.NeedKeyItemID) < curStage.NeedKeyItemCount)
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
            return;
        }

        stageManager.ResetStage();
        gameObject.SetActive(false);
    }

    public void MoveToShop()
    {
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetPopUpInactive(1);
        stageManager.SetStatus(IngameStatus.Planet);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetTabActive(3);
        gameObject.SetActive(false);
    }
}
