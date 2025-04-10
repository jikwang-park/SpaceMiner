using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : Singleton<TimeManager>
{

    public string url = "www.google.com";
    public DateTime currentTime;
    public DateTime CurrentTime
    {
        get
        {
            return currentTime;
        }
    }
    public IEnumerator GetServerTime(Action<DateTime> onTimeFetched)
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string date = request.GetResponseHeader("date");
                currentTime = DateTime.Parse(date).ToLocalTime();
                onTimeFetched?.Invoke(currentTime);
            }
            else
            {
                Debug.Log(request.error);
                DateTime defaultTime = DateTime.Now;
                onTimeFetched?.Invoke(defaultTime);
            }
        }
    }
}