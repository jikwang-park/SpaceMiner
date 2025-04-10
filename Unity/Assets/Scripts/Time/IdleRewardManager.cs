using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRewardManager : MonoBehaviour
{
    private StageSaveData stageSaveData;
    private readonly int defaultMaxMinute = 600;
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
    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            DateTime quitTime = SaveLoadManager.Data.quitTime;
            DateTime currentTime = TimeManager.Instance.GetEstimatedServerTime();
            int idleMinute = (int)(currentTime - quitTime).TotalMinutes;

            if(idleMinute > MaxMinute)
            {
                idleMinute = MaxMinute;
            }
            GetIdleReward(idleMinute);
        }
    }
    private void GetIdleReward(int idleTime)
    {
        // �������� �Ŵ���? ���� �ִ� ���������� ���´� -> �����ִ� �༺ �ݿ��� �̰� Ȱ�� �ؾ��ҵ�?
        // ä�� �Ŵ������� �����ִ� �༺�鿡 ��ġ���ִ� �κ��� �����Ͽ� ����ؼ� ä������ ���´�
        Dictionary<int, BigNumber> idleRewardsDict = new Dictionary<int, BigNumber>();

        int highestClearPlanet = stageSaveData.clearedPlanet;
        int highestClearStage = stageSaveData.clearedStage;

        var highestClearData = DataTableManager.StageTable.GetStageData(highestClearPlanet, highestClearStage);

        idleRewardsDict.Add(highestClearData.IdleRewardID, int.Parse(highestClearData.IdleRewardCount) * idleTime);


    }
}
