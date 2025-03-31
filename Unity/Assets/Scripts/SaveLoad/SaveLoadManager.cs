using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static string fileName = "SaveData.json";
    public static TotalSaveData LoadedData { get; private set; }
    public static event Action<TotalSaveData> onSaveRequested;
    
    public static void SaveGame()
    {
        TotalSaveData data = new TotalSaveData();

        onSaveRequested?.Invoke(data);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved to: " + filePath);
    }
    public static void LoadGame()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(filePath))
        {
            TotalSaveData defaultData = GetDefaultData();
            LoadedData = defaultData;
            string defaultJson = JsonConvert.SerializeObject(defaultData, Formatting.Indented);
            File.WriteAllText(filePath, defaultJson);
            return;
        }
        string json = File.ReadAllText(filePath);
        TotalSaveData loadedSaveData = JsonConvert.DeserializeObject<TotalSaveData>(json);

        if(loadedSaveData != null)
        {
            LoadedData = loadedSaveData;
            ItemManager.DoLoad();
        }
    }
    public static TotalSaveData GetDefaultData()
    {
        TotalSaveData defaultSaveData = new TotalSaveData();

        for(int i = (int)UnitTypes.Tanker; i < (int)UnitTypes.Healer + 1; i++)
        {
            defaultSaveData.inventorySaveData[(UnitTypes)i] = new InventorySaveData();
        }

        defaultSaveData.stageSaveData = new StageSaveData
        {
            currentPlanet = 1,
            currentStage = 1,
            highPlanet = 1,
            highStage = 1,
        };

        defaultSaveData.itemSaveData = new Dictionary<string, BigNumber>();

        return defaultSaveData;
    }
}
