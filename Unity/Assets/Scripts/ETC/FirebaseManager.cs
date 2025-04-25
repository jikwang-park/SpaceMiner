using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseAuth auth;
    private DatabaseReference root;
    private string userId;
    private long serverTimeOffsetMs;
    public async Task InitializeAsync()
    {
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
#if UNITY_EDITOR 
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false); 
#endif
        auth = FirebaseAuth.DefaultInstance;
        root = FirebaseDatabase.DefaultInstance.RootReference;

        var offsetRef = FirebaseDatabase.DefaultInstance.GetReference(".info/serverTimeOffset");
        offsetRef.ValueChanged += (s, e) => {
            if (long.TryParse(e.Snapshot.Value?.ToString(), out var ms))
                serverTimeOffsetMs = ms;
        };
        await SignInAnonymouslyAsync();
        await LoadFromFirebaseAsync();
    }

    private async Task SignInAnonymouslyAsync()
    {
        var result = await auth.SignInAnonymouslyAsync();
        this.userId = result.User.UserId;
        SaveLoadManager.onSaveRequested += SaveToFirebaseAsync;
        Debug.LogFormat("User signed in successfully: {0} ({1})",
            result.User.DisplayName, result.User.UserId);
    }
    private async void SaveToFirebaseAsync()
    {
        if(string.IsNullOrEmpty(userId))
        {
            return;
        }
        string json = JsonConvert.SerializeObject(SaveLoadManager.Data,
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            });

        await root.Child("users").Child(userId).Child("SaveData").SetRawJsonValueAsync(json);
    }
    private async Task LoadFromFirebaseAsync()
    {
        if (string.IsNullOrEmpty(userId))
        {
            return;
        }

        var snapshot = await root.Child("users").Child(userId).Child("SaveData").GetValueAsync();
        if (snapshot.Exists)
        {
            string json = snapshot.GetRawJsonValue();
            if (!string.IsNullOrEmpty(json))
            {
                SaveLoadManager.LoadGame(json);
            }
        }
        else
        {
            SaveLoadManager.SetDefaultData();
        }
    }
    private void OnApplicationQuit()
    {
        SetQuitTime();
    }
    public void SetQuitTime()
    {
        if(SaveLoadManager.Data is not null)
        {
            SaveLoadManager.Data.quitTime = GetFirebaseServerTime().ToLocalTime();
            SaveLoadManager.SaveGame();
        }
    }
    public DateTime GetFirebaseServerTime()
    {
        return DateTime.UtcNow.AddMilliseconds(serverTimeOffsetMs);
    }
}