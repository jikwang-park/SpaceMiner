using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonPopup : MonoBehaviour
{
    private StageManager stageManager;

    private List<DungeonTable.Data> subStages;

    private int index;

    [SerializeField]
    private LocalizationText nameText;

    [SerializeField]
    private TextMeshProUGUI selectedDifficulty;

    [SerializeField]
    private LocalizationText conditionStageText;

    [SerializeField]
    private LocalizationText conditionPowerText;

    [SerializeField]
    private LocalizationText keyText;

    [SerializeField]
    private LocalizationText clearRewardText;

    [SerializeField]
    private Button nextDifficultyButton;

    [SerializeField]
    private Button previousDifficultyButton;

    [SerializeField]
    private Button enterButton;

    private int maxStage;

    private bool Disabled;


    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void ShowPopup()
    {
        subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
        maxStage = SaveLoadManager.Data.stageSaveData.highestDungeon[Variables.currentDungeonType];
        SetIndex(maxStage - 1);
    }

    private void OnDisable()
    {
        Disabled = true;
    }

    private void OnEnable()
    {
        if (Disabled)
        {
            Disabled = false;
            ShowPopup();
        }
    }


    private void ShowData(int index)
    {
        var curStage = subStages[index];

        nameText.SetString(curStage.NameStringID);

        selectedDifficulty.text = curStage.Stage.ToString();
        keyText.SetStringArguments(curStage.NeedKeyItemCount.ToString(), ItemManager.GetItemAmount(curStage.NeedKeyItemID).ToString());
        conditionPowerText.SetStringArguments(new BigNumber(curStage.NeedPower).ToString());

        int highplanet = SaveLoadManager.Data.stageSaveData.highPlanet;
        if (SaveLoadManager.Data.stageSaveData.highPlanet != SaveLoadManager.Data.stageSaveData.clearedPlanet
             || SaveLoadManager.Data.stageSaveData.highStage != SaveLoadManager.Data.stageSaveData.clearedStage)
        {
            --highplanet;
        }

        conditionStageText.SetStringArguments(subStages[index].NeedClearPlanet.ToString());
        var itemNameID = DataTableManager.ItemTable.GetData(curStage.RewardItemID).NameStringID;
        var itemName = DataTableManager.StringTable.GetData(itemNameID);
        clearRewardText.SetStringArguments(itemName, curStage.ClearRewardItemCount.ToString());


        previousDifficultyButton.interactable = index > 0;
        nextDifficultyButton.interactable = index + 1 < maxStage && index < subStages.Count - 1;

        bool powerCondition = UnitCombatPowerCalculator.ToTalCombatPower > curStage.NeedPower;
        bool planetCondition = (SaveLoadManager.Data.stageSaveData.highPlanet > curStage.NeedClearPlanet)
                                 || (SaveLoadManager.Data.stageSaveData.highPlanet == SaveLoadManager.Data.stageSaveData.clearedPlanet
                                     && SaveLoadManager.Data.stageSaveData.highStage == SaveLoadManager.Data.stageSaveData.clearedStage);
        bool keyCondition = ItemManager.GetItemAmount(curStage.NeedKeyItemID) >= curStage.NeedKeyItemCount;

        enterButton.interactable = powerCondition && planetCondition && keyCondition;
    }

    private void SetIndex(int index)
    {
        this.index = index;

        ShowData(this.index);
    }

    //TODO: 인스펙터에서 스테이지 뒤로, 앞으로 버튼과 연결
    public void OnClickStage(bool isNext)
    {
        bool changed = false;

        if (isNext && index + 1 < maxStage && index < subStages.Count - 1)
        {
            ++index;
            changed = true;
        }
        else if (!isNext && index > 0)
        {
            --index;
            changed = true;
        }

        if (changed)
        {
            ShowData(index);
        }
    }

    //TODO: 인스펙터에서 엔터 버튼과 연결
    public void OnClickEnter()
    {
        Variables.currentDungeonStage = index + 1;
        stageManager.SetStatus(IngameStatus.Dungeon);
    }

    public void OnClickKeyGet()
    {
        ItemManager.AddItem(subStages[index].NeedKeyItemID, 1);
        ShowData(index);
    }
}
