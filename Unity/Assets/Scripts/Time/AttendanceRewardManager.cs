using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttendanceRewardManager : Singleton<AttendanceRewardManager>
{
    
    private Dictionary<int, AttendanceData> Attendances
    {
        get
        {
            return SaveLoadManager.Data.attendanceStates;
        }
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
        DateTime now = TimeManager.Instance.GetEstimatedServerTime();

       if(now.Date <= Attendances[attendanceId].lastClaimTime.Date)
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
