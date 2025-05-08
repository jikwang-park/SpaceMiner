using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : Singleton<TimeManager>
{
    public bool isDebug = false;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDebug = PlayerPrefs.GetInt("TimeManager.isDebug", 0) == 1;
    }
    public DateTime GetEstimatedServerTime()
    {
        return isDebug ? DateTime.Now : FirebaseManager.Instance.GetFirebaseServerTime().ToLocalTime();
    }
    public bool IsNewDay(DateTime beforeTime)
    {
        return beforeTime.Date < GetEstimatedServerTime().Date;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            FirebaseManager.Instance.SetQuitTime();
        }
    }
    public void ToggleDebugMode()
    {
        isDebug = !isDebug;
        PlayerPrefs.SetInt("TimeManager.isDebug", isDebug ? 1 : 0);
        PlayerPrefs.Save();
    }
}