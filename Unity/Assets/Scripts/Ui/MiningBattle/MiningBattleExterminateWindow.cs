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

    private DungeonTable.Data stageData;

    private StageManager stageManager;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }


    private void OnEnable()
    {
        var subStages = DataTableManager.DungeonTable.GetDungeonList(Variables.currentDungeonType);
        stageData = subStages[0];
        var lastDamage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
        var totalReward = DataTableManager.DamageDungeonRewardTable.GetRewards(lastDamage);

        foreach (var reward in totalReward)
        {
            var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
            itemIconGo.transform.SetParent(iconParent);
            var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
            rewardIcons.Add(rewardIcon);
            rewardIcon.SetItem(reward.Key, reward.Value);
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

    public void OnConfirm()
    {
        if (ItemManager.CanConsume(stageData.NeedKeyItemID, stageData.NeedKeyItemCount))
        {
            ItemManager.ConsumeItem(stageData.NeedKeyItemID, stageData.NeedKeyItemCount);

            var damage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
            var rewards = DataTableManager.DamageDungeonRewardTable.GetRewards(damage);

            foreach (var reward in rewards)
            {
                ItemManager.AddItem(reward.Key, reward.Value);
            }

            exterminateResult.Open(rewards);
        }
        else
        {
            requirementWindow.OpenNeedKey();
        }
    }
}
