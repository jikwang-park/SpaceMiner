using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/MineStageData", fileName = "Mine Stage Data")]
public class MineStageStatusMachineData : StageStatusMachineData
{
    [field: SerializeField]
    public string mine { get; private set; } = "Mine";
    [field: SerializeField]
    public string robot { get; private set; } = "MiningRobot";

    public Vector3 minePosition;

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
}
