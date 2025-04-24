using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleRewardManager : MonoBehaviour
{
    private StageSaveData stageSaveData;
    private readonly int defaultMaxMinute = 600;

    [SerializeField]
    private IdleRewardPopup idleRewardPopup;
    private int MaxMinute
    {
        get
        {
            int idleTypeBuildingLevel = SaveLoadManager.Data.buildingData.buildingLevels[BuildingTable.BuildingType.IdleTime];
            var addData = DataTableManager.BuildingTable.GetDatas(BuildingTable.BuildingType.IdleTime)[idleTypeBuildingLevel];
            var addMinute = addData.Value;
            return defaultMaxMinute + (int)addMinute;
        }
    }
    private void Awake()
    {
        stageSaveData = SaveLoadManager.Data.stageSaveData;
    }
    private void Start()
    {
        CalculateIdleReward();
    }
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            CalculateIdleReward();
        }
    }
    private void CalculateIdleReward()
    {
        DateTime quitTime = SaveLoadManager.Data.quitTime;
        DateTime currentTime = TimeManager.Instance.GetEstimatedServerTime();
        int idleMinute = (int)(currentTime - quitTime).TotalMinutes;
        if(idleMinute <= 0)
        {
            return;
        }

        if (idleMinute > MaxMinute)
        {
            idleMinute = MaxMinute;
        }
        GetIdleReward(idleMinute);
    }
    private void GetIdleReward(int idleTime)
    {
        // �������� �Ŵ���? ���� �ִ� ���������� ���´� -> �����ִ� �༺ �ݿ��� �̰� Ȱ�� �ؾ��ҵ�?
        // ä�� �Ŵ������� �����ִ� �༺�鿡 ��ġ���ִ� �κ��� �����Ͽ� ����ؼ� ä������ ���´�
        Dictionary<int, BigNumber> idleRewardsDict = new Dictionary<int, BigNumber>();

        int highestClearPlanet = stageSaveData.clearedPlanet;
        int highestClearStage = stageSaveData.clearedStage;

        if(highestClearPlanet == 1 && highestClearStage == 0)
        {
            return;
        }

        var highestClearData = DataTableManager.StageTable.GetStageData(highestClearPlanet, highestClearStage);

        idleRewardsDict.Add(highestClearData.IdleRewardItemID, int.Parse(highestClearData.IdleRewardItemCount) * idleTime);

        Dictionary<int, BigNumber> idleMiningReward = MiningRobotInventoryManager.GetIdleRewardOpenPlanet();

        foreach(var kvp in idleMiningReward)
        {
            if(idleRewardsDict.ContainsKey(kvp.Key))
            {
                idleRewardsDict[kvp.Key] += kvp.Value * idleTime;
            }
            else 
            {
                idleRewardsDict.Add(kvp.Key, kvp.Value * idleTime);
            }
        }
        var processedIdleRewards = idleRewardsDict.Where((e) => e.Value > 0).ToList();
        if(processedIdleRewards.Count > 0)
        {
            foreach (var rewardItem in processedIdleRewards)
            {
                var value = rewardItem.Value / 1000;
                ItemManager.AddItem(rewardItem.Key, rewardItem.Value);
                Debug.Log($"Get Idle Reward - {rewardItem.Key} : {rewardItem.Value}");
            }
            FirebaseManager.Instance.SetQuitTime();
            DisplayRewardUI(processedIdleRewards);
        }
    }

    private void DisplayRewardUI(List<KeyValuePair<int, BigNumber>> rewards)
    {
        idleRewardPopup.gameObject.SetActive(true);
        idleRewardPopup.DisplayIdleReward(rewards);
    }
}
