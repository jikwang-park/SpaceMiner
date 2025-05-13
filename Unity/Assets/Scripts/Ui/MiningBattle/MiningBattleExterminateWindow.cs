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

    public event System.Action OnExterminate;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }


    private void OnEnable()
    {
        var subStages = DataTableManager.MiningBattleTable.GetDatas(Variables.planetMiningID);
        var clearedStageIndex = SaveLoadManager.Data.stageSaveData.ClearedMineStage[Variables.planetMiningStage] - 1;
        stageData = subStages[clearedStageIndex];

        AddIcon(stageData.Reward1ItemID, stageData.Reward1ItemCount);

        for (int i = 0; i < stageData.Reward2ItemIDs.Length; ++i)
        {
            AddIcon(stageData.Reward2ItemIDs[i], 0, stageData.Reward2ItemCounts[i]);
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
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            SortedList<int, BigNumber> gotItems = new SortedList<int, BigNumber>();

            ItemManager.AddItem(stageData.Reward1ItemID, stageData.Reward1ItemCount);
            gotItems.Add(stageData.Reward1ItemID, stageData.Reward1ItemCount);

            for (int i = 0; i < stageData.Reward2ItemIDs.Length; ++i)
            {
                if (Random.value < stageData.Reward2ItemProbabilities[i])
                {
                    int itemCount = Random.Range(0, stageData.Reward2ItemCounts[i]) + 1;
                    ItemManager.AddItem(stageData.Reward2ItemIDs[i], itemCount);
                    gotItems.Add(stageData.Reward2ItemIDs[i], itemCount);
                }
            }
            ++SaveLoadManager.Data.mineBattleData.mineBattleCount;
            SaveLoadManager.Data.mineBattleData.lastClearTime = TimeManager.Instance.GetEstimatedServerTime();

            exterminateResult.Open(gotItems);
            OnExterminate?.Invoke();
        }
        else
        {
            requirementWindow.OpenMiningFullCount();
        }
    }
}
