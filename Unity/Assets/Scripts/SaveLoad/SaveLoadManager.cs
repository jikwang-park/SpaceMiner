using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static string fileName = "SaveData.json";

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
            Debug.LogWarning("Save file not found at: " + filePath);
            return;
        }
        string json = File.ReadAllText(filePath);
        TotalSaveData loadedSaveData = JsonConvert.DeserializeObject<TotalSaveData>(json);


    }
}
