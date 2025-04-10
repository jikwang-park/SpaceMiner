using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRewardManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        SaveLoadManager.SaveGame();
    }
    private void OnApplicationPause(bool pause)
    {
        SaveLoadManager.SaveGame();
    }
}
