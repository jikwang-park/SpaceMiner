using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageStatusMachine
{
    protected StageManager stageManager;

    protected float stageEndTime;

    protected StageStatusMachine(StageManager stageManager)
    {
        this.stageManager = stageManager;
    }

    public bool IsActive { get; protected set; } = false;

    public abstract void SetStageData(StageStatusMachineData stageMachineData);
    public abstract void Start();
    public abstract void Update();
    public abstract void Exit();
    public abstract void Reset();
    public virtual void SetActive(bool isActive)
    {
        if (IsActive == isActive)
        {
            return;
        }

        IsActive = isActive;
    }
}
