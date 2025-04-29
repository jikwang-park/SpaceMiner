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
        switch (Variables.currentDungeonType)
        {
            case 1:
                var subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
                int maxStage = SaveLoadManager.Data.stageSaveData.clearedDungeon[Variables.currentDungeonType];
                stageData = subStages[maxStage - 1];
                var itemdata = DataTableManager.ItemTable.GetData(stageData.RewardItemID);
                rewardImage.SetSprite(itemdata.SpriteID);
                rewardItemName.SetString(itemdata.NameStringID);
                rewardText.text = stageData.ClearRewardItemCount.ToString();
                break;
            case 2:
                stageData = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType)[0];
                var damage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
                var lastData = DataTableManager.DamageDungeonRewardTable.GetData(damage);
                var itemData = DataTableManager.ItemTable.GetData(lastData.RewardItemID);

                rewardImage.SetSprite(itemData.SpriteID);
                rewardItemName.SetString(itemData.NameStringID);
                rewardText.text = lastData.RewardItemCount.ToString();
                break;
        }
    }

    public void OnConfirm()
    {
        if (ItemManager.CanConsume(stageData.NeedKeyItemID, stageData.NeedKeyItemCount))
        {
            ItemManager.ConsumeItem(stageData.NeedKeyItemID, stageData.NeedKeyItemCount);

            switch (Variables.currentDungeonType)
            {
                case 1:
                    ItemManager.AddItem(stageData.RewardItemID, stageData.ClearRewardItemCount);
                    break;
                case 2:
                    var damage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
                    var rewards = DataTableManager.DamageDungeonRewardTable.GetRewards(damage);

                    foreach (var reward in rewards)
                    {
                        ItemManager.AddItem(reward.Key, reward.Value);
                    }
                    break;
            }

            gameObject.SetActive(false);
        }
        else
        {
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
        }
    }
}
