using TMPro;
using UnityEngine;

public class Dungeon1ExterminateWindow : MonoBehaviour
{
    [SerializeField]
    private AddressableImage rewardImage;

    [SerializeField]
    private LocalizationText rewardItemName;

    [SerializeField]
    private TextMeshProUGUI rewardText;

    [SerializeField]
    private LocalizationText stageText;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    [SerializeField]
    private DungeonExterminateResult resultWindow;

    private DungeonTable.Data stageData;

    private void OnEnable()
    {
        var subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
        int maxStage = SaveLoadManager.Data.stageSaveData.clearedDungeon[Variables.currentDungeonType];
        stageData = subStages[maxStage - 1];
        var itemdata = DataTableManager.ItemTable.GetData(stageData.RewardItemID);
        rewardImage.SetSprite(itemdata.SpriteID);
        rewardItemName.SetString(itemdata.NameStringID);
        rewardText.text = stageData.ClearRewardItemCount.ToString();
        stageText.SetStringArguments(stageData.Stage.ToString());
    }

    public void OnConfirm()
    {
        if (ItemManager.CanConsume(stageData.NeedKeyItemID, stageData.NeedKeyItemCount))
        {
            ItemManager.ConsumeItem(stageData.NeedKeyItemID, stageData.NeedKeyItemCount);

            ItemManager.AddItem(stageData.RewardItemID, stageData.ClearRewardItemCount);
            resultWindow.Open(stageData.RewardItemID, stageData.ClearRewardItemCount);
        }
        else
        {
            requirementWindow.OpenNeedKey();
        }
    }
}
