using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBattleExterminateWindow : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    [SerializeField]
    private DungeonExterminateResult exterminateResult;

    [SerializeField]
    private Transform iconParent;

    private MiningBattleTable.Data stageData;

    private StageManager stageManager;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }


    private void OnEnable()
    {
        var subStages = DataTableManager.MiningBattleTable.GetDatas(Variables.planetMiningID);
        var clearedStageIndex = SaveLoadManager.Data.stageSaveData.ClearedMineStage[Variables.planetMiningStage];
        stageData = subStages[clearedStageIndex];

        AddIcon(stageData.Reward1ItemID, stageData.Reward1ItemCount);

        for (int i = 0; i < stageData.Reward2ItemIDs.Length; ++i)
        {
            AddIcon(stageData.Reward2ItemIDs[i], 0, stageData.Reward2ItemIDs[i]);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
    }

    private void AddIcon(int itemID, BigNumber amount)
    {
        var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
        itemIconGo.transform.SetParent(iconParent);
        var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
        rewardIcons.Add(rewardIcon);
        rewardIcon.SetItem(itemID, amount);
    }

    private void AddIcon(int itemID, BigNumber minAmount, BigNumber maxAmount)
    {
        var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
        itemIconGo.transform.SetParent(iconParent);
        var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
        rewardIcons.Add(rewardIcon);
        rewardIcon.SetItem(itemID, minAmount, maxAmount);
    }

    public void OnConfirm()
    {
        //if (ItemManager.CanConsume(stageData.NeedKeyItemID, stageData.NeedKeyItemCount))
        //{
        //    ItemManager.ConsumeItem(stageData.NeedKeyItemID, stageData.NeedKeyItemCount);

        //    var damage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
        //    var rewards = DataTableManager.DamageDungeonRewardTable.GetRewards(damage);

        //    foreach (var reward in rewards)
        //    {
        //        ItemManager.AddItem(reward.Key, reward.Value);
        //    }

        //    exterminateResult.Open(rewards);
        //}
        //else
        //{
        //    requirementWindow.OpenNeedKey();
        //}
    }
}
