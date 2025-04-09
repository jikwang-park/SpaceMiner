using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : Singleton<TimeManager>
{
    public string url = "";
    public IEnumerator GetServerTime(Action<DateTime> onTimeFetched)
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(date).ToLocalTime();
                onTimeFetched?.Invoke(dateTime);
            }
            else
            {
                Debug.Log(request.error);
                onTimeFetched?.Invoke(DateTime.MinValue);
            }
        }
    }
}