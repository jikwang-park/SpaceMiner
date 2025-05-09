using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseAuth auth;
    private DatabaseReference root;
    public FirebaseUser User { get; private set; }
    private long serverTimeOffsetMs;
    public async Task InitializeAsync()
    {
        await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();
#if UNITY_EDITOR 
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false); 
#endif
        auth = FirebaseAuth.DefaultInstance;
        root = FirebaseDatabase.DefaultInstance.RootReference;

        if (auth.CurrentUser == null)
        {
            var result = await auth.SignInAnonymouslyAsync();
            User = result.User;
            Debug.Log($"Signed in anonymously: {User.UserId}");
        }
        else if (auth.CurrentUser != null && auth.CurrentUser != User)
        {
            User = auth.CurrentUser;
            Debug.Log($"Restored session for user: {User.UserId}");
        }

        var offsetRef = FirebaseDatabase.DefaultInstance.GetReference(".info/serverTimeOffset");
        offsetRef.ValueChanged += (s, e) => {
            if (long.TryParse(e.Snapshot.Value?.ToString(), out var ms))
                serverTimeOffsetMs = ms;
        };

        auth.StateChanged += AuthStateChanged;
        SaveLoadManager.onSaveRequested += SaveToFirebaseAsync;
        await LoadFromFirebaseAsync();
        UnitCombatPowerCalculator.onCombatPowerChanged += DoCombatPowerChanged;
    }
    private async void AuthStateChanged(object sender, EventArgs e)
    {
        var fbUser = auth.CurrentUser;
        if (fbUser == null && User == null)
        {
            var result = await auth.SignInAnonymouslyAsync();
            User = result.User;
            Debug.Log($"Signed in anonymously: {User.UserId}");
        }
        else if (fbUser != null && fbUser != User)
        {
            User = fbUser;
            Debug.Log($"Restored session for user: {User.UserId}");
        }
    }
    private async void SaveToFirebaseAsync()
    {
        if (User == null) return;

        string json = JsonConvert.SerializeObject(SaveLoadManager.Data, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        });

        await root.Child("users")
            .Child(User.UserId)
            .Child("SaveData")
            .SetRawJsonValueAsync(json);
    }
    private async Task LoadFromFirebaseAsync()
    {
        if (User == null) return;

        try
        {
            var snap = await root.Child("users")
                .Child(User.UserId)
                .Child("SaveData")
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
        catch
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
        SaveLoadManager.SetDefaultData();
    }
    public async void DoCombatPowerChanged()
    {
        await UpdateCombatPowerToLeaderBoard();
    }
    public async void UpdateLeaderBoard()
    {
        await UpdateCombatPowerToLeaderBoard();
        await UpdateDungeonDamageToLeaderBoard();
        await UpdateHighestStageToLeaderBoard();

    }
    public Task UpdateCombatPowerToLeaderBoard()
    {
        if(User == null)
        {
            throw new InvalidOperationException("로그인 필요");
        }

        BigNumber combatPower = UnitCombatPowerCalculator.TotalCombatPower;
        string displayCombatPower = combatPower.ToString();
        string sortKeyCombatPower = combatPower.GetSortKey();
        string nickname = User.DisplayName;

        var entry = new
        {
            displayCombatPower,
            sortKeyCombatPower,
            name = nickname,
            timeStamp = ServerValue.Timestamp
        };

        return root
            .Child("leaderboard")
            .Child("CombatPower")
            .Child(User.UserId)
            .SetRawJsonValueAsync(JsonConvert.SerializeObject(entry));
    }
    public Task UpdateDungeonDamageToLeaderBoard()
    {
        if (User == null)
        {
            throw new InvalidOperationException("로그인 필요");
        }
        BigNumber damage = SaveLoadManager.Data.stageSaveData.dungeonTwoDamage;
        string displayDamage = damage.ToString();
        string sortKeyDamage = damage.IsZero ? "0" : damage.GetSortKey();
        string nickname = User.DisplayName;

        var entry = new
        {
            displayDamage,
            sortKeyDamage,
            name = nickname,
            timeStamp = ServerValue.Timestamp
        };

        return root
            .Child("leaderboard")
            .Child("DungeonDamage")
            .Child(User.UserId)
            .SetRawJsonValueAsync(JsonConvert.SerializeObject(entry));
    }
    public Task UpdateHighestStageToLeaderBoard()
    {
        if (User == null)
        {
            throw new InvalidOperationException("로그인 필요");
        }
        int stageId = DataTableManager.StageTable.GetStageData(SaveLoadManager.Data.stageSaveData.highPlanet, SaveLoadManager.Data.stageSaveData.highStage).ID;
        string nickname = User.DisplayName;
        var entry = new
        {
            stage = stageId,
            name = nickname,
            timeStamp = ServerValue.Timestamp
        };

        return root
            .Child("leaderboard")
            .Child("HighestStage")
            .Child(User.UserId)
            .SetRawJsonValueAsync(JsonConvert.SerializeObject(entry));
    }
    public async Task<List<LeaderBoardEntry>> GetTopCombatPowerAsync(int topN)
    {
        var refCombatPower = root
            .Child("leaderboard")
            .Child("CombatPower");

        var snap = await refCombatPower
            .OrderByChild("sortKeyCombatPower")
            .LimitToLast(topN)
            .GetValueAsync();

        var list = new List<LeaderBoardEntry>();
        foreach (var child in snap.Children)
        {
            var ds = child;
            list.Add(new LeaderBoardEntry
            {
                uid = ds.Key,
                name = ds.Child("name").Value?.ToString() ?? "",
                display = ds.Child("displayCombatPower").Value?.ToString() ?? "",
                sortKey = ds.Child("sortKeyCombatPower").Value?.ToString() ?? "",
                timestamp = (long)(ds.Child("timestamp").Value ?? 0L)
            });
        }

        var result = list
            .OrderBy(e => e.sortKey)
            .ThenBy(e => e.timestamp)
            .Reverse()
            .ToList();

        return result;
    }
    public async Task<List<LeaderBoardEntry>> GetTopHighestStageAsync(int topN)
    {
        var refHighestStage = root
            .Child("leaderboard")
            .Child("HighestStage");

        var snap = await refHighestStage
            .OrderByChild("stage")
            .LimitToLast(topN)
            .GetValueAsync();

        var list = new List<LeaderBoardEntry>();
        foreach (var child in snap.Children)
        {
            var ds = child;
            list.Add(new LeaderBoardEntry
            {
                uid = ds.Key,
                name = ds.Child("name").Value?.ToString() ?? "",
                display = ds.Child("stage").Value?.ToString() ?? "",
                sortKey = ds.Child("stage").Value?.ToString() ?? "",
                timestamp = (long)(ds.Child("timestamp").Value ?? 0L)
            });
        }

        var result = list
                    .OrderBy(e => e.sortKey)
                    .ThenBy(e => e.timestamp)
                    .Reverse()
                    .ToList();

        return result;
    }
    public async Task<List<LeaderBoardEntry>> GetTopDungeonDamageAsync(int topN)
    {
        var refCombatPower = root
            .Child("leaderboard")
            .Child("DungeonDamage");

        var snap = await refCombatPower
            .OrderByChild("sortKeyDamage")
            .LimitToLast(topN)
            .GetValueAsync();

        var list = new List<LeaderBoardEntry>();
        foreach (var child in snap.Children)
        {
            var ds = child;
            list.Add(new LeaderBoardEntry
            {
                uid = ds.Key,
                name = ds.Child("name").Value?.ToString() ?? "",
                display = ds.Child("displayDamage").Value?.ToString() ?? "",
                sortKey = ds.Child("sortKeyDamage").Value?.ToString() ?? "",
                timestamp = (long)(ds.Child("timestamp").Value ?? 0L)
            });
        }

        var result = list
                    .OrderBy(e => e.sortKey)
                    .ThenBy(e => e.timestamp)
                    .Reverse()
                    .ToList();

        return result;
    }
    public async Task<MyRankEntry> GetMyCombatPowerRankAsync()
    {
        var mySnap = await root
            .Child("leaderboard")
            .Child("CombatPower")
            .Child(User.UserId)
            .GetValueAsync();

        if (!mySnap.Exists)
        {
            return new MyRankEntry { rank = -1, myEntry = null };
        }

        var userId = mySnap.Key;
        var nameNode = mySnap.Child("name");
        string name = (nameNode.Exists && nameNode.Value != null) ? nameNode.Value.ToString() : "";

        var combatPowerNode = mySnap.Child("displayCombatPower");
        string display = (combatPowerNode.Exists && combatPowerNode.Value != null) ? combatPowerNode.Value.ToString() : "";

        var entry = new LeaderBoardEntry { uid = userId, display = display, name = name };

        string mySortKey = mySnap.Child("sortKeyCombatPower").Value as string;

        var snap = await root
            .Child("leaderboard")
            .Child("CombatPower")
            .OrderByChild("sortKeyCombatPower")
            .StartAt(mySortKey)
            .GetValueAsync();

        MyRankEntry myRank = new MyRankEntry { rank = (int)snap.ChildrenCount, myEntry = entry };
        return myRank;
    }
    public async Task<MyRankEntry> GetMyHighestStageRankAsync()
    {
        var mySnap = await root
            .Child("leaderboard")
            .Child("HighestStage")
            .Child(User.UserId)
            .GetValueAsync();

        if (!mySnap.Exists)
        {
            return new MyRankEntry { rank = -1, myEntry = null };
        }

        var userId = mySnap.Key;
        var nameNode = mySnap.Child("name");
        string name = (nameNode.Exists && nameNode.Value != null) ? nameNode.Value.ToString() : "";

        var stageNode = mySnap.Child("stage");
        string display = (stageNode.Exists && stageNode.Value != null) ? stageNode.Value.ToString() : "";

        var entry = new LeaderBoardEntry
        {
            uid = userId,
            display = display,
            name = name
        };

        string mySortKey = mySnap.Child("stage").Value as string;

        var snap = await root
            .Child("leaderboard")
            .Child("HighestStage")
            .OrderByChild("stage")
            .StartAt(mySortKey)
            .GetValueAsync();

        MyRankEntry myRank = new MyRankEntry { rank = (int)snap.ChildrenCount, myEntry = entry };
        return myRank;
    }

    public async Task<MyRankEntry> GetMyDungeonDamageRankAsync()
    {
        var mySnap = await root
            .Child("leaderboard")
            .Child("DungeonDamage")
            .Child(User.UserId)
            .GetValueAsync();

        if (!mySnap.Exists)
        {
            return new MyRankEntry { rank = -1, myEntry = null };
        }

        var userId = mySnap.Key;
        var nameNode = mySnap.Child("name");
        string name = (nameNode.Exists && nameNode.Value != null) ? nameNode.Value.ToString() : "";

        var damageNode = mySnap.Child("sortKeyDamage");
        string display = (damageNode.Exists && damageNode.Value != null) ? damageNode.Value.ToString() : "";

        var entry = new LeaderBoardEntry
        {
            uid = userId,
            display = display,
            name = name
        };

        string mySortKey = mySnap.Child("sortKeyDamage").Value as string;

        var snap = await root
            .Child("leaderboard")
            .Child("DungeonDamage")
            .OrderByChild("sortKeyDamage")
            .StartAt(mySortKey)
            .GetValueAsync();

        MyRankEntry myRank = new MyRankEntry { rank = (int)snap.ChildrenCount, myEntry = entry };
        return myRank;
    }
}