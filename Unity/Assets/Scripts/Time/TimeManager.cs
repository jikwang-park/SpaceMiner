using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : Singleton<TimeManager>
{
    public string url = "www.google.com";
    private TimeSpan serverTimeOffset = TimeSpan.Zero;
    public bool isDebug = false;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isDebug = PlayerPrefs.GetInt("TimeManager.isDebug", 0) == 1;
    }
    public IEnumerator SyncWithServer()
    {
        if(isDebug)
        {
            yield break;
        }

        yield return StartCoroutine(GetServerTime((DateTime serverTime) =>
        {
            if (serverTime != DateTime.MinValue)
            {
                serverTimeOffset = serverTime - DateTime.Now;  
            }
        }));
    }

    public IEnumerator GetServerTime(Action<DateTime> onTimeFetched)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string date = request.GetResponseHeader("date");
            DateTime serverTime = DateTime.Parse(date).ToLocalTime();
            onTimeFetched?.Invoke(serverTime);
        }
        else
        {
            Debug.Log(request.error);
            onTimeFetched?.Invoke(DateTime.Now);
        }
    }
    public DateTime GetEstimatedServerTime()
    {
        return isDebug ? DateTime.Now : DateTime.Now + serverTimeOffset;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            StartCoroutine(SyncWithServer());
        }
    }
    private void OnApplicationQuit()
    {
        SetQuitTime();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SetQuitTime();
        }
    }
    public void SetQuitTime()
    {
        SaveLoadManager.Data.quitTime = GetEstimatedServerTime();
        SaveLoadManager.SaveGame();
    }
    public void ToggleDebugMode()
    {
        isDebug = !isDebug;
        PlayerPrefs.SetInt("TimeManager.isDebug", isDebug ? 1 : 0);
        PlayerPrefs.Save();
    }
}