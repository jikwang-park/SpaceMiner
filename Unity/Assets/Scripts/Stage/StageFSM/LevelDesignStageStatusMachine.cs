using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDesignStageStatusMachine : StageStatusMachine
{
    protected enum Status
    {
        Play,
        Stop,
    }

    public float stageTime;
    public float weight;
    public int waveLength;
    public int waveTarget;
    public float respawnDistance;
    public CorpsTable.Data[] corpsDatas;

    private int CurrentWave;
    private Status status;
    protected float remainingTime;

    private event System.Action OnStageEnd;

    private bool initilized = false;

    public LevelDesignStageStatusMachine(StageManager stageManager) : base(stageManager)
    {
        OnStageEnd += stageManager.StageEnd;
    }

    public override void Start()
    {
        CurrentWave = 1;
        stageEndTime = Time.time + stageTime;
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);

        UnitSpawn();
        stageManager.CameraManager.SetCameraOffset();

        stageManager.StageMonsterManager.SetWeight(weight);
        NextWave();
    }

    public override void Exit()
    {
        stageManager.SetStatus(IngameStatus.Planet);
    }

    public override void Reset()
    {
        stageManager.StageMonsterManager.StopMonster();
        stageManager.UnitPartyManager.UnitDespawn();
        stageManager.StageUiManager.HPBarManager.ClearHPBar();
        stageManager.StageMonsterManager.ClearMonster();
    }

    public override void SetStageData(StageStatusMachineData stageMachineData)
    {
    }


    public override void Update()
    {
        float currentTime = Time.time;
        switch (status)
        {
            case Status.Play:
                UpdateTimer(currentTime);
                break;
            case Status.Stop:
                break;
        }

    }

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);

        if (isActive)
        {
            if (!initilized)
            {
                Init();
            }
            stageManager.StageMonsterManager.OnMonsterCleared += OnMonsterCleared;
        }
        else
        {
            stageManager.StageMonsterManager.OnMonsterCleared -= OnMonsterCleared;
        }
    }

    private void Init()
    {
        initilized = true;
        stageTime = Variables.PlanetTime;
        corpsDatas = new CorpsTable.Data[4];
        for (int i = 0; i < corpsDatas.Length; ++i)
        {
            corpsDatas[i] = new CorpsTable.Data();
        }
        SetStageData(1, 1);
    }

    private void UpdateTimer(float currentTime)
    {
        remainingTime = stageEndTime - currentTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            StageEnd();
        }
        if (stageManager.UnitPartyManager.UnitCount == 0)
        {
            StageEnd();
        }

        stageManager.StageUiManager.IngameUIManager.SetTimer(remainingTime);
    }

    private void NextWave()
    {
        stageManager.UnitPartyManager.ResetStatus();
        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);

        status = Status.Play;

        Transform unit = stageManager.UnitPartyManager.GetFirstLineUnitTransform();

        if (unit is not null)
        {
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * respawnDistance, corpsDatas[CurrentWave - 1]);
        }
        else
        {
            stageManager.StageMonsterManager.Spawn(Vector3.zero, corpsDatas[CurrentWave - 1]);
        }

        stageManager.StageUiManager.IngameUIManager.SetWaveText(CurrentWave);
        ++CurrentWave;
    }

    private void OnMonsterCleared()
    {
        if (CurrentWave > waveLength)
        {
            StageEnd();
            return;
        }
        NextWave();
    }

    private void StageEnd()
    {
        status = Status.Stop;
        stageManager.StageUiManager.HPBarManager.ClearHPBar();
    }

    protected void UnitSpawn()
    {
        stageManager.UnitPartyManager.UnitSpawn();
        stageManager.UnitPartyManager.ResetUnitHealth();
        stageManager.UnitPartyManager.ResetSkillCoolTime();
        stageManager.UnitPartyManager.ResetStatus();
    }

    public void SetStageData(int planet, int stage)
    {
        var stageData = DataTableManager.StageTable.GetStageData(planet, stage);
        if (stageData is null)
        {
            return;
        }
        weight = stageData.Weight;
        stageManager.StageMonsterManager.SetWeight(weight);
        var waveData = DataTableManager.WaveTable.GetData(stageData.WaveID);
        respawnDistance = waveData.RespawnDistance;
        waveLength = waveData.CorpsIDs.Length;

        for (int i = 0; i < waveData.CorpsIDs.Length; ++i)
        {
            var tableData = DataTableManager.CorpsTable.GetData(waveData.CorpsIDs[i]);
            corpsDatas[i].FrontSlots = tableData.FrontSlots;
            corpsDatas[i].NormalMonsterID = tableData.NormalMonsterID;
            corpsDatas[i].BackSlots = tableData.BackSlots;
            corpsDatas[i].RangedMonsterID = tableData.RangedMonsterID;
            corpsDatas[i].BossMonsterID = tableData.BossMonsterID;

            int count = tableData.NormalMonsterIDs.Length;
            corpsDatas[i].NormalMonsterIDs = new int[count];
            Array.Copy(tableData.NormalMonsterIDs, corpsDatas[i].NormalMonsterIDs, count);
            count = tableData.RangedMonsterIDs.Length;
            corpsDatas[i].RangedMonsterIDs = new int[count];
            Array.Copy(tableData.RangedMonsterIDs, corpsDatas[i].RangedMonsterIDs, count);
        }
    }
}
