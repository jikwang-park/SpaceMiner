using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttendanceRewardManager : Singleton<AttendanceRewardManager>
{
    private readonly int attendanceOpenId = 4103000;
    private Dictionary<int, AttendanceData> Attendances
    {
        get
        {
            return SaveLoadManager.Data.attendanceStates;
        }
    }
    [SerializeField]
    private GameObject attendancePopup;
    public void StartPopup()
    {
        if (HasAnyClaimableReward() && IsOpenedContents())
        {
            attendancePopup.gameObject.SetActive(true);
        }
    }
    public bool HasAnyClaimableReward()
    {
        foreach (var attendance in DataTableManager.AttendanceTable.GetList())
        {
            int id = attendance.ID;
            int dayIndex = Attendances[id].currentIndex;

            if (CanClaim(id, dayIndex))
            { 
                return true;
            }
        }
        return false;
    }
    public void CheckDailyReset()
    {
        DateTime now = TimeManager.Instance.GetEstimatedServerTime();

        foreach(var attendance in DataTableManager.AttendanceTable.GetList())
        {
            int attendanceId = attendance.ID;
            var attendanceData = Attendances[attendanceId];

            if (now.Date > attendanceData.lastClaimTime.Date)
            {
                if(attendance.IsRepeat == 1 && attendanceData.currentIndex > attendance.Period)
                {
                    attendanceData.currentIndex = 1;
                }
            }
        }
    }
    public bool IsClaimed(int attendanceId, int dayIndex)
    {
        return dayIndex < Attendances[attendanceId].currentIndex;
    }
    public bool CanClaim(int attendanceId, int dayIndex)
    {
        if(!Attendances.ContainsKey(attendanceId))
        {
            return false;
        }

        var attendanceData = DataTableManager.AttendanceTable.GetList().Find((e) => e.ID == attendanceId);

       if(!TimeManager.Instance.IsNewDay(Attendances[attendanceId].lastClaimTime.Date))
        {
            return false;
        }

       if(dayIndex != Attendances[attendanceId].currentIndex)
        {
            return false;
        }

       if(dayIndex > attendanceData.Period && attendanceData.IsRepeat != 1)
       {
            return false;
       }

       
        return true;
    }
    public bool IsOpenedContents()
    {
        var data = SaveLoadManager.Data.stageSaveData;

        int stageId = DataTableManager.ContentsOpenTable.GetData(attendanceOpenId);
        var stageData = DataTableManager.StageTable.GetData(stageId);

        if(stageData == null)
        {
            return false;
        }

        if(data.highPlanet >= stageData.Planet && data.highStage >= stageData.Stage)
        {
            return true;
        }

        return false;
    }
    public void Claim(int attendanceId, int dayIndex)
    {
        if(!CanClaim(attendanceId, dayIndex))
        { 
            return; 
        }

        var attendanceData = DataTableManager.AttendanceTable.GetList().Find((e) => e.ID == attendanceId);
        var rewardData = DataTableManager.AttendanceRewardTable.GetData(attendanceId, dayIndex);
        DateTime now = TimeManager.Instance.GetEstimatedServerTime();
        int rewardItemId = rewardData.RewardItemID;
        BigNumber rewardAmount = rewardData.RewardItemCount;

        ItemManager.AddItem(rewardItemId, rewardAmount);

        Attendances[attendanceId].lastClaimTime = now;
        Attendances[attendanceId].currentIndex = Attendances[attendanceId].currentIndex < attendanceData.Period ? Attendances[attendanceId].currentIndex + 1
            : (attendanceData.IsRepeat == 1 ? 1 : attendanceData.Period);
    }
}
