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
    private FirebaseUser user;
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

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        SaveLoadManager.onSaveRequested += SaveToFirebaseAsync;

        await LoadFromFirebaseAsync();
    }
    private async void AuthStateChanged(object sender, EventArgs e)
    {
        var fbUser = auth.CurrentUser;
        if (fbUser == null && user == null)
        {
            var result = await auth.SignInAnonymouslyAsync();
            user = result.User;
            Debug.Log($"Signed in anonymously: {user.UserId}");
        }
        else if (fbUser != null && fbUser != user)
        {
            user = fbUser;
            Debug.Log($"Restored session for user: {user.UserId}");
        }
    }
    private async void SaveToFirebaseAsync()
    {
        if (user == null) return;

        string json = JsonConvert.SerializeObject(SaveLoadManager.Data, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        });

        await root.Child("users").Child(user.UserId).Child("SaveData")
                  .SetRawJsonValueAsync(json);
    }
    private async Task LoadFromFirebaseAsync()
    {
        if (user == null) return;

        var snap = await root.Child("users").Child(user.UserId).Child("SaveData")
                             .GetValueAsync();
        if (snap.Exists && !string.IsNullOrEmpty(snap.GetRawJsonValue()))
        {
            SaveLoadManager.LoadGame(snap.GetRawJsonValue());
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
    private void OnDestroy()
    {
        if (auth != null) auth.StateChanged -= AuthStateChanged;
    }
    public async Task ResetUserDataAsync()
    {
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser == null)
            throw new System.InvalidOperationException("로그인된 사용자가 없습니다.");

        string uid = auth.CurrentUser.UserId;
        var dbRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(uid);

        await dbRef.RemoveValueAsync();

    }

}