using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DungeonExterminate : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI diffText;

    [SerializeField]
    private AddressableImage rewardImage;

    [SerializeField]
    private TextMeshProUGUI rewardText;

    [SerializeField]
    private GameObject notEnoughKeyWindow;

    private DungeonTable.Data stageData;

    private void OnEnable()
    {
        var subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
        int maxStage = SaveLoadManager.Data.stageSaveData.clearedDungeon[Variables.currentDungeonType];
        stageData = subStages[maxStage - 1];
        if (stageData.Stage == maxStage)
        {
            diffText.text = maxStage.ToString();
        }
        else
        {
            diffText.text = "";
        }
        var itemdata = DataTableManager.ItemTable.GetData(stageData.RewardItemID);
        rewardImage.SetSprite(itemdata.SpriteID);
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
            notEnoughKeyWindow.SetActive(true);
        }
    }
}
