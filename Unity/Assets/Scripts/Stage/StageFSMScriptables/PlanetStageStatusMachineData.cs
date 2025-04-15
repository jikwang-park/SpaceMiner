using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/PlanetStageData", fileName = "Planet Stage Data")]
public class PlanetStageStatusMachineData : StageStatusMachineData
{
    public float spawnDistance = 20f;
    public float spawnDelay = 1f;
    public float stageEndDelay = 1f;
}
