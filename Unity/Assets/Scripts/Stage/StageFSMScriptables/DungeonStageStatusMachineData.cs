using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/DungeonStageData", fileName = "Dungeon Stage Data")]
public class DungeonStageStatusMachineData : StageStatusMachineData
{
    public float spawnDistance = 10f;
    public float spawnDelay = 1f;
    public float stageEndDelay = 1f;
}
