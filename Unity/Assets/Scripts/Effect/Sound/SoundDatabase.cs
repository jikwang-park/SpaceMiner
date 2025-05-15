using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Scriptables/SoundDatabase")]
public class SoundDatabase : ScriptableObject
{
    [SerializedDictionary("AudioID", "AudioAddressableKey")]
    public SerializedDictionary<string, string> audioDictionary;
}
