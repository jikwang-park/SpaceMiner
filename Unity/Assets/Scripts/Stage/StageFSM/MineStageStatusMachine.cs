using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineStageStatusMachine : StageStatusMachine
{
    private MineStageStatusMachineData stageMachineData;

    private Mine mine;
    private int planetID;
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

            MiningRobotInventoryManager.onEquipRobot += OnEquipChanged;
        }
        else
        {
            stageManager.CameraManager.enabled = true;
            stageManager.CameraManager.SetCameraRotation();
            stageManager.CameraManager.SetCameraOffset();

            mine.Release();
            mine = null;
            for (int i = 0; i < robotControllers.Length; ++i)
            {
                robotControllers[i]?.Release();
                robotControllers[i] = null;
            }
        }
    }
    public override void Exit()
    {
        stageManager.SetStatus(IngameStatus.Planet);
    }

    public override void Reset()
    {
        if (Variables.planetMiningID == planetID)
        {
            return;
        }
        mine.Release();
        mine = null;
        for (int i = 0; i < robotControllers.Length; ++i)
        {
            robotControllers[i]?.Release();
            robotControllers[i] = null;
        }
        InitStage();
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
        planetID = Variables.planetMiningID;
        var planetData = DataTableManager.PlanetTable.GetData(planetID);
        var prefabAddress = DataTableManager.AddressTable.GetData(planetData.PrefabID);
        var mineGo = stageManager.ObjectPoolManager.Get(prefabAddress);
        mineGo.transform.parent = null;
        mine = mineGo.GetComponent<Mine>();
        var equipments = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet;

        if (!equipments.ContainsKey(planetID))
        {
            return;
        }

        for (int i = 0; i < equipments[planetID].Length; ++i)
        {
            if (equipments[planetID][i].isEmpty)
            {
                continue;
            }
            var robotData = DataTableManager.RobotTable.GetData(equipments[planetID][i].miningRobotId);
            var robotAddress = DataTableManager.AddressTable.GetData(robotData.PrefabID);
            var robotGo = stageManager.ObjectPoolManager.Get(robotAddress);
            robotGo.transform.parent = null;
            robotGo.transform.position = mine.GetSpawnPoint(i).position;
            robotControllers[i] = robotGo.GetComponent<MiningRobotController>();
            robotControllers[i].Init(planetID, equipments[planetID][i].miningRobotId, i);
            robotControllers[i].SetOreStorage(mine.GetOre(i), mine.GetStorage(i));
        }
    }

    private void OnEquipChanged(int planetID)
    {
        if (!IsActive)
        {
            return;
        }

        if (Variables.planetMiningID != planetID)
        {
            return;
        }

        var equipments = MiningRobotInventoryManager.Inventory.equipmentSlotsToPlanet;

        if (!equipments.ContainsKey(planetID))
        {
            return;
        }

        for (int i = 0; i < equipments[planetID].Length; ++i)
        {
            bool robotCreated = robotControllers[i] is not null;
            if (equipments[planetID][i].isEmpty)
            {
                if (robotCreated)
                {
                    robotControllers[i].Release();
                    robotControllers[i] = null;
                }
                continue;
            }
            if (robotCreated && robotControllers[i].RobotData.ID == equipments[planetID][i].miningRobotId)
            {
                continue;
            }
            if (robotCreated)
            {
                robotControllers[i].Release();
                robotControllers[i] = null;
            }
            var robotData = DataTableManager.RobotTable.GetData(equipments[planetID][i].miningRobotId);
            var robotAddress = DataTableManager.AddressTable.GetData(robotData.PrefabID);
            var robotGo = stageManager.ObjectPoolManager.Get(robotAddress);
            robotGo.transform.parent = null;
            robotGo.transform.position = mine.GetSpawnPoint(i).position;
            robotControllers[i] = robotGo.GetComponent<MiningRobotController>();
            robotControllers[i].Init(planetID, equipments[planetID][i].miningRobotId, i);
            robotControllers[i].SetOreStorage(mine.GetOre(i), mine.GetStorage(i));
        }
    }
}
