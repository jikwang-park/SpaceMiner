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
        // 스테이지 매니저? 에서 최대 스테이지를 얻어온다 -> 열려있는 행성 반영도 이걸 활용 해야할듯?
        // 채굴 매니저에서 열려있는 행성들에 배치돼있는 로봇과 연관하여 계산해서 채굴량을 얻어온다
        Dictionary<int, BigNumber> idleRewardsDict = new Dictionary<int, BigNumber>();

        int highestClearPlanet = stageSaveData.clearedPlanet;
        int highestClearStage = stageSaveData.clearedStage;

        var highestClearData = DataTableManager.StageTable.GetStageData(highestClearPlanet, highestClearStage);

        idleRewardsDict.Add(highestClearData.IdleRewardID, int.Parse(highestClearData.IdleRewardCount) * idleTime);


    }
}
