using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelDesignStageStatusMachine : StageStatusMachine
{
    protected enum Status
    {
        Play,
        Stop,
    }

    public class WaveMonsterData
    {
        public int[] slots = new int[3];
        public BigNumber[] attack = new BigNumber[3];
        public BigNumber[] maxHp = new BigNumber[3];
        public float[] attackRange = new float[3];
        public float[] attackSpeed = new float[3];
        public float[] moveSpeed = new float[3];
    }

    public class SkillData
    {
        public float[] coolTime = new float[3];
        public float[] ratio = new float[3];
        public float[] etc = new float[3];
    }

    public float stageTime;
    public float[] weight = new float[3];
    public int waveLength;
    public int waveTarget;
    public float respawnDistance;
    public CorpsTable.Data[] corpsDatas;
    public WaveMonsterData[] waveMonsterDatas;
    public SkillData skillData;
    public MonsterSkillTable.Data monsterSkillData;

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
        waveMonsterDatas = new WaveMonsterData[4];
        for (int i = 0; i < corpsDatas.Length; ++i)
        {
            corpsDatas[i] = new CorpsTable.Data();
            waveMonsterDatas[i] = new WaveMonsterData();
        }
        SetStageData(1, 1);

        skillData = new SkillData();

        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            string name = type.ToString() + Grade.Normal.ToString();
            int defaultId = DataTableManager.DefaultDataTable.GetID(name);
            switch (type)
            {
                case UnitTypes.Tanker:
                    var tankerSkillData = DataTableManager.TankerSkillTable.GetData(defaultId);
                    skillData.coolTime[0] = tankerSkillData.CoolTime;
                    skillData.ratio[0] = tankerSkillData.ShieldRatio;
                    skillData.etc[0] = tankerSkillData.Duration;
                    break;
                case UnitTypes.Dealer:
                    var dealerSkillData = DataTableManager.DealerSkillTable.GetData(defaultId);
                    skillData.coolTime[1] = dealerSkillData.CoolTime;
                    skillData.ratio[1] = dealerSkillData.DamageRatio;
                    skillData.etc[1] = dealerSkillData.MonsterMaxTarget;
                    break;
                case UnitTypes.Healer:
                    var healerSkillData = DataTableManager.HealerSkillTable.GetData(defaultId);
                    skillData.coolTime[2] = healerSkillData.CoolTime;
                    skillData.ratio[2] = healerSkillData.HealRatio;
                    break;
            }
        }

        monsterSkillData = new MonsterSkillTable.Data();
        monsterSkillData.AttackRatio = 2;
        monsterSkillData.SkillRange = 10;
        monsterSkillData.CoolTime = 5;
        monsterSkillData.MaxTargetCount = 2;
        monsterSkillData.TargetPriority = (TargetPriority)1;
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
            stageManager.StageMonsterManager.Spawn(unit.position + Vector3.forward * respawnDistance, waveMonsterDatas[CurrentWave - 1], monsterSkillData);
        }
        else
        {
            stageManager.StageMonsterManager.Spawn(Vector3.zero, waveMonsterDatas[CurrentWave - 1], monsterSkillData);
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
        UnitSkillSet();
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
        weight[0] = stageData.AtkWeight;
        weight[1] = stageData.HpWeight;
        weight[2] = stageData.GoldWeight;
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

            waveMonsterDatas[i].slots[0] = corpsDatas[i].FrontSlots;
            waveMonsterDatas[i].slots[1] = corpsDatas[i].BackSlots;
            waveMonsterDatas[i].slots[2] = tableData.BossMonsterID != 0 ? 1 : 0;

            if (corpsDatas[i].FrontSlots > 0)
            {
                var monsterTableData = DataTableManager.MonsterTable.GetData(corpsDatas[i].NormalMonsterIDs[0]);
                waveMonsterDatas[i].attack[0] = monsterTableData.Attack;
                waveMonsterDatas[i].maxHp[0] = monsterTableData.HP;
                waveMonsterDatas[i].attackRange[0] = monsterTableData.AttackRange;
                waveMonsterDatas[i].attackSpeed[0] = monsterTableData.AttackSpeed;
                waveMonsterDatas[i].moveSpeed[0] = monsterTableData.MoveSpeed;
            }
            if (corpsDatas[i].BackSlots > 0)
            {
                var monsterTableData = DataTableManager.MonsterTable.GetData(corpsDatas[i].RangedMonsterIDs[0]);
                waveMonsterDatas[i].attack[1] = monsterTableData.Attack;
                waveMonsterDatas[i].maxHp[1] = monsterTableData.HP;
                waveMonsterDatas[i].attackRange[1] = monsterTableData.AttackRange;
                waveMonsterDatas[i].attackSpeed[1] = monsterTableData.AttackSpeed;
                waveMonsterDatas[i].moveSpeed[1] = monsterTableData.MoveSpeed;
            }
            if (waveMonsterDatas[i].slots[2] > 0)
            {
                var monsterTableData = DataTableManager.MonsterTable.GetData(corpsDatas[i].BossMonsterID);
                waveMonsterDatas[i].attack[2] = monsterTableData.Attack;
                waveMonsterDatas[i].maxHp[2] = monsterTableData.HP;
                waveMonsterDatas[i].attackRange[2] = monsterTableData.AttackRange;
                waveMonsterDatas[i].attackSpeed[2] = monsterTableData.AttackSpeed;
                waveMonsterDatas[i].moveSpeed[2] = monsterTableData.MoveSpeed;
            }
        }
    }

    private void UnitSkillSet()
    {
        foreach (UnitTypes type in Enum.GetValues(typeof(UnitTypes)))
        {
            var unitTransform = stageManager.UnitPartyManager.GetUnit(type);
            var unit = unitTransform.GetComponent<Unit>();
            unit.Skill.SetCoolTime(skillData.coolTime[(int)type - 1]);
            unit.Skill.SetRatio(skillData.ratio[(int)type - 1]);
            unit.Skill.SetEtcValue(skillData.etc[(int)type - 1]);
        }
    }
}
