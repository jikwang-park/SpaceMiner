using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttendanceRewardManager : MonoBehaviour
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
    public bool CanClaim(int attendanceId)
    {
        if(!Attendances.ContainsKey(attendanceId))
        {
            return false;
        }

        var attendanceData = DataTableManager.AttendanceTable.GetList().Find((e) => e.ID == attendanceId);
        DateTime now = TimeManager.Instance.GetEstimatedServerTime();

        bool canClaimDay = now.Date > Attendances[attendanceId].lastClaimTime.Date;
        bool canClaimIndex = Attendances[attendanceId].currentIndex < attendanceData.Period || attendanceData.IsRepeat == 1;

        return canClaimDay && canClaimIndex;
    }
    public void Claim(int attendanceId)
    {
        if(!CanClaim(attendanceId))
        { 
            return; 
        }

        var attendanceData = DataTableManager.AttendanceTable.GetList().Find((e) => e.ID == attendanceId);
        var rewardData = DataTableManager.AttendanceRewardTable.GetData(attendanceId, Attendances[attendanceId].currentIndex);
        int rewardItemId = rewardData.RewardItemID;
        BigNumber rewardAmount = rewardData.RewardItemCount;

        ItemManager.AddItem(rewardItemId, rewardAmount);

        Attendances[attendanceId].lastClaimTime = TimeManager.Instance.GetEstimatedServerTime();
        Attendances[attendanceId].currentIndex = Attendances[attendanceId].currentIndex < attendanceData.Period ? Attendances[attendanceId].currentIndex + 1
            : (attendanceData.IsRepeat == 1 ? 1 : attendanceData.Period);
    }
}
