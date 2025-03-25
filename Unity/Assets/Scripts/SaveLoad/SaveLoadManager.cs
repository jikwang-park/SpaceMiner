using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public static string fileName = "SaveData.json";
    public event Action<TotalSaveData> OnSaveRequested;
    public event Action<TotalSaveData> OnLoadRequested;

    public void SaveGame()
    {
        TotalSaveData data = new TotalSaveData();

        OnSaveRequested?.Invoke(data);

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved to: " + filePath);
    }
}
