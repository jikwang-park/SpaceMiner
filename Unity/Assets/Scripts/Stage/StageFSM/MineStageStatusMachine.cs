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
        var mineGo = stageManager.ObjectPoolManager.Get(stageMachineData.mine);
        mine = mineGo.GetComponent<Mine>();

        var robotGo = stageManager.ObjectPoolManager.Get(stageMachineData.robot);
        robotControllers[0] = robotGo.GetComponent<MiningRobotController>();
        robotControllers[0].SetOreStorage(mine.GetOre(0), mine.GetStorage(0));
    }
}
