using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/MineStageData", fileName = "Mine Stage Data")]
public class MineStageStatusMachineData : StageStatusMachineData
{
    public Vector3 minePosition;

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
}
