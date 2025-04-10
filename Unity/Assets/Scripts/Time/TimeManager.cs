using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : Singleton<TimeManager>
{
    public string url = "www.google.com";
    private TimeSpan serverTimeOffset = TimeSpan.Zero;

    public IEnumerator SyncWithServer()
    {
        yield return StartCoroutine(GetServerTime((DateTime serverTime) =>
        {
            if (serverTime != DateTime.MinValue)
            {
                serverTimeOffset = serverTime - DateTime.Now;
                Debug.Log("���� ����ȭ �Ϸ�, ������: " + serverTimeOffset);
            }
            else
            {
                Debug.LogWarning("���� �ð� ����ȭ ����");
            }
        }));
    }

    public IEnumerator GetServerTime(Action<DateTime> onTimeFetched)
    {
        UnityWebRequest request = UnityWebRequest.Get("http://www.google.com");
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
        return DateTime.Now + serverTimeOffset;
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
        SaveLoadManager.Data.quitTime = GetEstimatedServerTime();
        SaveLoadManager.SaveGame();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveLoadManager.Data.quitTime = GetEstimatedServerTime();
            SaveLoadManager.SaveGame();
        }
    }
}