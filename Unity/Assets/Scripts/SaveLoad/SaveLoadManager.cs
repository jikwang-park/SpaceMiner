using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveDataVC = SaveDataV1;

public static class SaveLoadManager
{
    public static int SaveDataVersion { get; private set; } = 1;
    public static SaveDataVC Data { get; set; }

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
        TypeNameHandling = TypeNameHandling.All,
    };
    static SaveLoadManager()
    {
        if (!LoadGame())
        {
            Data = GetDefaultData();
            SaveGame();
        }
    }
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
        try
        {
            var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);
            while (saveData.Version < SaveDataVersion)
            {
                saveData = saveData.VersionUp();
            }
            Data = saveData as SaveDataVC;
        }
        catch
        {
            Data = GetDefaultData();
        }

        return true;
    }
    public static SaveDataVC GetDefaultData()
    {
        SaveDataVC defaultSaveData = new SaveDataVC();

        defaultSaveData.stageSaveData = new StageSaveData
        {
            currentPlanet = 1,
            currentStage = 1,
            highPlanet = 1,
            highStage = 1,
        };

        defaultSaveData.itemSaveData = new Dictionary<int, BigNumber>();

        defaultSaveData.inventorySaveData = new Dictionary<UnitTypes, InventorySaveData>();
        var datasByType = DataTableManager.SoldierTable.GetTypeDictionary();

        foreach (var type in datasByType.Keys)
        {
            InventorySaveData inventoryData = new InventorySaveData();
            inventoryData.inventoryType = type;

            foreach (var soldierData in datasByType[type])
            {
                InventoryElementSaveData elementData = new InventoryElementSaveData()
                {
                    soldierId = soldierData.ID,
                    isLocked = true,
                    grade = soldierData.Rating,
                    count = 0,
                    level = 0
                };
                inventoryData.elements.Add(elementData);
            }
            inventoryData.elements[0].isLocked = false;
            inventoryData.elements[0].count = 1;
            inventoryData.equipElementID = inventoryData.elements[0].soldierId;
            defaultSaveData.inventorySaveData[type] = inventoryData;
        }

        return defaultSaveData;
    }
}
