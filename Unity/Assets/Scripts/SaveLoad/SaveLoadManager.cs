using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using SaveDataVC = SaveDataV3;

public static class SaveLoadManager
{
    public static int SaveDataVersion { get; private set; } = 3;
    public static SaveDataVC Data { get; private set; }

    public static string fileName = "SaveData.json";
    public static event Action onSaveRequested;
    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
    };
    public static void SaveGame()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        onSaveRequested?.Invoke();

        string json = JsonConvert.SerializeObject(Data, settings);
        string filePath = Path.Combine(SaveDirectory, fileName);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved to: " + filePath);
    }
    public static bool LoadGame()
    {
        string filePath = Path.Combine(SaveDirectory, fileName);
        if (!File.Exists(filePath))
        {
            return false;
        }
        string json = File.ReadAllText(filePath);

        LoadGame(json);

        return true;
    }
    public static void LoadGame(string json)
    {
        try
        {
            var jObj = JObject.Parse(json);

            int version = jObj.Value<int>("Version");
            SaveData saveData;
            switch (version)
            {
                case 1:
                    saveData = JsonConvert.DeserializeObject<SaveDataV1>(json);
                    break;
                case 2:
                    saveData = JsonConvert.DeserializeObject<SaveDataV2>(json);
                    break;
                case 3:
                    saveData = JsonConvert.DeserializeObject<SaveDataV3>(json);
                    break;
                default:
                    Debug.LogWarning($"Unknown SaveData version {version}, creating fresh default.");
                    SetDefaultData();
                    return;
            }
            while (saveData.Version < SaveDataVersion)
            {
                saveData = saveData.VersionUp();
            }
            Data = saveData as SaveDataVC;
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            SetDefaultData();
        }
    }
    public static void SetDefaultData()
    {
        SaveDataVC defaultSaveData = new SaveDataVC();

        defaultSaveData.stageSaveData = StageSaveData.CreateDefault();

        defaultSaveData.questProgressData = QuestProgressData.CreateDefault();

        defaultSaveData.itemSaveData = new Dictionary<int, BigNumber>();

        defaultSaveData.soldierInventorySaveData = new Dictionary<UnitTypes, SoldierInventoryData>();
        var datasByType = DataTableManager.SoldierTable.GetTypeDictionary();

        foreach (var type in datasByType.Keys)
        {
            defaultSaveData.soldierInventorySaveData[type] = SoldierInventoryData.CreateDefault(type, datasByType[type]);
        }
        defaultSaveData.miningRobotInventorySaveData = MiningRobotInventoryData.CreateDefault();

        defaultSaveData.unitStatUpgradeData = UnitStatUpgradeData.CreateDefault();
        defaultSaveData.unitSkillUpgradeData = UnitSkillUpgradeData.CreateDefault();
        defaultSaveData.buildingData = BuildingData.CreateDefault();
        defaultSaveData.dungeonKeyShopData = DungeonKeyShopData.CreateDefault();
        defaultSaveData.quitTime = DateTime.Now;

        Data = defaultSaveData;
    }
    public static void ResetStatUpgradeData()
    {
        Data.unitStatUpgradeData = UnitStatUpgradeData.CreateDefault();
    }
    public static void ResetSoldierInventoryData()
    {
        Data.soldierInventorySaveData = new Dictionary<UnitTypes, SoldierInventoryData>();
        var datasByType = DataTableManager.SoldierTable.GetTypeDictionary();

        foreach (var type in datasByType.Keys)
        {
            Data.soldierInventorySaveData[type] = SoldierInventoryData.CreateDefault(type, datasByType[type]);
        }
    }
    public static void ResetSkillUpgradeData()
    {
        Data.unitSkillUpgradeData = UnitSkillUpgradeData.CreateDefault();
    }
    public static void ResetBuildingUpgradeData()
    {
        Data.buildingData = BuildingData.CreateDefault();
    }
    public static void ResetMiningRobotInventoryData()
    {
        Data.miningRobotInventorySaveData = MiningRobotInventoryData.CreateDefault();
    }
    public static void ResetDungeonKeyShopData()
    {
        Data.dungeonKeyShopData = DungeonKeyShopData.CreateDefault();
    }
    public static void ResetStageSaveData()
    {
        Data.stageSaveData = StageSaveData.CreateDefault();
    }
    public static void ResetItemSaveData()
    {
        Data.itemSaveData = new Dictionary<int, BigNumber>();
    }
    public static void UnlockAllStage()
    {
        var lastStageData = DataTableManager.StageTable.GetLastStage();

        Data.stageSaveData.clearedPlanet = lastStageData.Planet;
        Data.stageSaveData.clearedStage = lastStageData.Stage;
        Data.stageSaveData.highPlanet = lastStageData.Planet;
        Data.stageSaveData.highStage = lastStageData.Stage;

        var lastDungeonData = DataTableManager.DungeonTable.GetLastStages();
        foreach(var data in lastDungeonData)
        {
            Data.stageSaveData.highestDungeon[data.Key] = data.Value.Stage;
        }
    }
}
