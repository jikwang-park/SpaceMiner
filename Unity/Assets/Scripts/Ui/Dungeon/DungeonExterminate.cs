using TMPro;
using UnityEngine;

public class DungeonExterminate : MonoBehaviour
{
    [SerializeField]
    private AddressableImage rewardImage;

    [SerializeField]
    private LocalizationText rewardItemName;

    [SerializeField]
    private TextMeshProUGUI rewardText;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

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
    }

    public void OnConfirm()
    {
        if (ItemManager.CanConsume(stageData.NeedKeyItemID, stageData.NeedKeyItemCount))
        {
            ItemManager.ConsumeItem(stageData.NeedKeyItemID, stageData.NeedKeyItemCount);
            ItemManager.AddItem(stageData.RewardItemID, stageData.ClearRewardItemCount);
            gameObject.SetActive(false);
        }
        else
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
        }
    }
}
