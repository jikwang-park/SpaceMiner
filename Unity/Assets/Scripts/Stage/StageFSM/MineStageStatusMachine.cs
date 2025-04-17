using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineStageStatusMachine : StageStatusMachine
{
    private MineStageStatusMachineData stageMachineData;

    private Mine mine;
    private MiningRobotController[] robotControllers = new MiningRobotController[2];

    public MineStageStatusMachine(StageManager stageManager) : base(stageManager)
    {
    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
        this.stageMachineData = (MineStageStatusMachineData)stageMachineData;
    }

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        if (IsActive)
        {
            Start();
            stageManager.CameraManager.enabled = false;
            stageManager.CameraManager.SetCameraRotation(stageMachineData.cameraRotation);
            stageManager.CameraManager.SetCameraOffset(stageMachineData.cameraPosition);
        }
        else
        {
            stageManager.CameraManager.enabled = true;
            stageManager.CameraManager.SetCameraRotation();
            stageManager.CameraManager.SetCameraOffset();

            mine.Release();
            foreach (var controller in robotControllers)
            {
                controller?.Release();
            }
        }
    }
    public override void Exit()
    {
        stageManager.SetStatus(IngameStatus.Planet);
    }

    public override void Reset()
    {

    }

    public override void Start()
    {
        InitStage();
    }

    public override void Update()
    {

    }

    protected void InitStage()
    {
        var planetData = DataTableManager.PlanetTable.GetData(Variables.planetMiningID);
        var prefabAddress = DataTableManager.AddressTable.GetData(planetData.PrefabID);
        var mineGo = stageManager.ObjectPoolManager.Get(prefabAddress);
        mineGo.transform.parent = null;
        mine = mineGo.GetComponent<Mine>();

        var equipments = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet;

        if (!equipments.ContainsKey(Variables.planetMiningID))
        {
            return;
        }

        for (int i = 0; i < equipments[Variables.planetMiningID].Length; ++i)
        {
            if (equipments[Variables.planetMiningID][i].isEmpty)
            {
                continue;
            }
            var robotData = DataTableManager.RobotTable.GetData(equipments[Variables.planetMiningID][i].miningRobotId);
            var robotAddress = DataTableManager.AddressTable.GetData(robotData.PrefabID);
            var robotGo = stageManager.ObjectPoolManager.Get(robotAddress);
            robotGo.transform.parent = null;
            robotGo.transform.position = mine.GetSpawnPoint(i).position;
            robotControllers[i] = robotGo.GetComponent<MiningRobotController>();
            robotControllers[i].Init(Variables.planetMiningID, equipments[Variables.planetMiningID][i].miningRobotId, i);
            robotControllers[i].SetOreStorage(mine.GetOre(i), mine.GetStorage(i));
        }
    }
}
