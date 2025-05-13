using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningBattleResultWindow : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private LocalizationText stageText;

    [SerializeField]
    private LocalizationText remainingText;

    [SerializeField]
    private LocalizationText clearMessageText;

    [SerializeField]
    private LocalizationText waitTimeText;

    [SerializeField]
    private GameObject clearView;

    [SerializeField]
    private GameObject defeatView;

    [SerializeField]
    private Transform rewardRow;

    private StageManager stageManager;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private float endTime;

    private void Update()
    {
        if (endTime < Time.time)
        {
            gameObject.SetActive(false);
        }
        waitTimeText.SetStringArguments((endTime - Time.time).ToString("F0"));
    }


    private void OnDisable()
    {
        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
    }

    public void ShowClear(MiningBattleTable.Data data, List<(int itemid, BigNumber amount)> gainItem)
    {
        stageText.SetStringArguments(data.Stage.ToString());
        gameObject.SetActive(true);
        clearView.SetActive(true);
        defeatView.SetActive(false);
        remainingText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
        clearMessageText.SetString(Defines.PlanetStageClearStringID);

        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }

        foreach (var reward in gainItem)
        {
            var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
            itemIconGo.transform.SetParent(rewardRow);
            var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
            rewardIcons.Add(rewardIcon);
            rewardIcon.SetItem(reward.itemid, reward.amount);
        }


        endTime = Time.time + Defines.MiningBattleResultWait;
    }

    public void ShowDefeat(MiningBattleTable.Data data)
    {
        gameObject.SetActive(true);
        defeatView.SetActive(true);
        clearView.SetActive(false);
        remainingText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
        clearMessageText.SetString(Defines.StageFailStringID);
        endTime = Time.time + Defines.MiningBattleResultWait;
    }

    public void NextStage()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            ++Variables.planetMiningStage;
            stageManager.MiningBattleStart();
        }
    }

    public void RetryStage()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            stageManager.MiningBattleStart();
        }
    }
}
