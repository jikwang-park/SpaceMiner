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
    private async void Awake()
    {
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        auth = FirebaseAuth.DefaultInstance;
        root = FirebaseDatabase.DefaultInstance.RootReference;

        var offsetRef = FirebaseDatabase.DefaultInstance.GetReference(".info/serverTimeOffset");

        offsetRef.ValueChanged += (s, e) => {
            if (long.TryParse(e.Snapshot.Value?.ToString(), out var ms))
            {
                serverTimeOffsetMs = ms;
            }
        };
    }
    public async void SetGame()
    {
        await SignInAnonymously();
        await LoadFromFirebase();
    }
    private async Task SignInAnonymously()
    {
        await auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            this.userId = result.User.UserId;
            SaveLoadManager.onSaveRequested += SaveToFirebase;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
    private async void SaveToFirebase()
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
    private async Task LoadFromFirebase()
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
        SaveLoadManager.Data.quitTime = GetFirebaseServerTime().ToLocalTime();
        SaveLoadManager.SaveGame();
    }
    public DateTime GetFirebaseServerTime()
    {
        return DateTime.UtcNow.AddMilliseconds(serverTimeOffsetMs);
    }
}