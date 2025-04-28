using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesignTab : MonoBehaviour
{
    public enum WaveType
    {
        MonsterCount,
        Attack,
        MaxHP,
        AttackRange,
        AttackSpeed,
        MoveSpeed,
    }

    private LevelDesignStageStatusMachine machine;

    private StageManager stageManager;

    private UnitTypes selectedUnit;

    [SerializeField]
    private TMP_InputField stageInput;
    [SerializeField]
    private TMP_InputField spawnDistanceInput;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private TMP_InputField weightInput;
    [SerializeField]
    private TMP_InputField timeLimitInput;

    [SerializeField]
    private SerializedDictionary<StatType, TMP_InputField[]> statInputs;

    [SerializeField]
    private SerializedDictionary<WaveType, TMP_InputField[]> waveInputs;

    private float exitButtonTime;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        machine = (LevelDesignStageStatusMachine)stageManager.GetStage(IngameStatus.LevelDesign);
        exitButtonTime = float.MinValue;
        UnitCombatPowerCalculator.onCombatPowerChanged += RefreshUnitInput;
        RefreshText();
        RefreshUnitInput();
        RefreshMonsterWave();
        SetUnitInputEvent();
        SetMonsterInputEvent();
    }

    public void SetStage()
    {
        if (string.IsNullOrEmpty(stageInput.text))
        {
            return;
        }
        var split = stageInput.text.Split('-');
        if (split.Length != 2)
        {
            return;
        }
        if (int.TryParse(split[0], out int planet) && int.TryParse(split[1], out int stage))
        {
            machine.SetStageData(planet, stage);
            RefreshText();
        }
    }

    public void SetWeight()
    {
        if (string.IsNullOrEmpty(weightInput.text))
        {
            return;
        }
        if (float.TryParse(weightInput.text, out float weight))
        {
            machine.weight = weight;
            RefreshText();
        }
    }

    public void SetTimeLimit()
    {
        if (string.IsNullOrEmpty(timeLimitInput.text))
        {
            return;
        }
        if (float.TryParse(timeLimitInput.text, out float timeLimit))
        {
            machine.stageTime = timeLimit;
            RefreshText();
        }
    }

    public void SetSpawnDistance()
    {
        if (string.IsNullOrEmpty(spawnDistanceInput.text))
        {
            return;
        }
        if (float.TryParse(spawnDistanceInput.text, out float spawnDistance))
        {
            machine.respawnDistance = spawnDistance;
            RefreshText();
        }
    }

    public void StartStage()
    {
        machine.Start();
    }

    public void StopStage()
    {
        machine.Reset();
    }

    public void ExitButtonClicked()
    {
        if (exitButtonTime + 1f > Time.time)
        {
            stageManager.SetStatus(IngameStatus.Planet);
            return;
        }
        exitButtonTime = Time.time;
    }

    public void WaveCountUpDown(bool isUp)
    {
        if (isUp && machine.waveLength < machine.corpsDatas.Length)
        {
            ++machine.waveLength;
            RefreshText();
            RefreshMonsterWave();
        }
        if (!isUp && machine.waveLength > 1)
        {
            --machine.waveLength;
            if (machine.waveTarget >= machine.waveLength)
            {
                --machine.waveTarget;
            }
            RefreshText();
            RefreshMonsterWave();
        }
    }

    public void WaveTargetUpDown(bool isUp)
    {
        if (isUp && machine.waveTarget + 1 < machine.waveLength)
        {
            ++machine.waveTarget;
            RefreshText();
            RefreshMonsterWave();
        }
        if (!isUp && machine.waveTarget > 0)
        {
            --machine.waveTarget;
            RefreshText();
            RefreshMonsterWave();
        }
    }

    private void RefreshText()
    {
        spawnDistanceInput.text = machine.respawnDistance.ToString("F0");
        weightInput.text = machine.weight.ToString("F2");
        waveText.text = $"{machine.waveTarget + 1} / {machine.waveLength}";
        timeLimitInput.text = machine.stageTime.ToString("F0");
    }

    private void RefreshUnitInput()
    {
        var stats = UnitCombatPowerCalculator.statsDictionary;

        statInputs[StatType.Attack][0].text = stats[UnitTypes.Tanker].soldierAttack.ToString();
        statInputs[StatType.Attack][1].text = stats[UnitTypes.Dealer].soldierAttack.ToString();
        statInputs[StatType.Attack][2].text = stats[UnitTypes.Healer].soldierAttack.ToString();

        statInputs[StatType.Defence][0].text = stats[UnitTypes.Tanker].soldierDefense.ToString();
        statInputs[StatType.Defence][1].text = stats[UnitTypes.Dealer].soldierDefense.ToString();
        statInputs[StatType.Defence][2].text = stats[UnitTypes.Healer].soldierDefense.ToString();

        statInputs[StatType.MaxHP][0].text = stats[UnitTypes.Tanker].soldierMaxHp.ToString();
        statInputs[StatType.MaxHP][1].text = stats[UnitTypes.Dealer].soldierMaxHp.ToString();
        statInputs[StatType.MaxHP][2].text = stats[UnitTypes.Healer].soldierMaxHp.ToString();

        statInputs[StatType.CriticalPossibility][0].text = stats[UnitTypes.Tanker].criticalPossibility.ToString();
        statInputs[StatType.CriticalPossibility][1].text = stats[UnitTypes.Dealer].criticalPossibility.ToString();
        statInputs[StatType.CriticalPossibility][2].text = stats[UnitTypes.Healer].criticalPossibility.ToString();

        statInputs[StatType.CriticalMultiplier][0].text = stats[UnitTypes.Tanker].criticalMultiplier.ToString();
        statInputs[StatType.CriticalMultiplier][1].text = stats[UnitTypes.Dealer].criticalMultiplier.ToString();
        statInputs[StatType.CriticalMultiplier][2].text = stats[UnitTypes.Healer].criticalMultiplier.ToString();

        statInputs[StatType.AttackSpeed][0].text = (100f / stats[UnitTypes.Tanker].coolDown).ToString();
        statInputs[StatType.AttackSpeed][1].text = (100f / stats[UnitTypes.Dealer].coolDown).ToString();
        statInputs[StatType.AttackSpeed][2].text = (100f / stats[UnitTypes.Healer].coolDown).ToString();

        statInputs[StatType.MoveSpeed][0].text = stats[UnitTypes.Tanker].moveSpeed.ToString();
        statInputs[StatType.MoveSpeed][1].text = stats[UnitTypes.Dealer].moveSpeed.ToString();
        statInputs[StatType.MoveSpeed][2].text = stats[UnitTypes.Healer].moveSpeed.ToString();

        statInputs[StatType.AttackRange][0].text = stats[UnitTypes.Tanker].attackRange.ToString();
        statInputs[StatType.AttackRange][1].text = stats[UnitTypes.Dealer].attackRange.ToString();
        statInputs[StatType.AttackRange][2].text = stats[UnitTypes.Healer].attackRange.ToString();
    }

    private void RefreshMonsterWave()
    {
        var waveMonsterData = machine.waveMonsterDatas[machine.waveTarget];

        waveInputs[WaveType.MonsterCount][0].text = waveMonsterData.slots[0].ToString();
        waveInputs[WaveType.MonsterCount][1].text = waveMonsterData.slots[1].ToString();
        waveInputs[WaveType.MonsterCount][2].text = waveMonsterData.slots[2].ToString();

        waveInputs[WaveType.Attack][0].text = waveMonsterData.attack[0].ToString();
        waveInputs[WaveType.Attack][1].text = waveMonsterData.attack[1].ToString();
        waveInputs[WaveType.Attack][2].text = waveMonsterData.attack[2].ToString();

        waveInputs[WaveType.MaxHP][0].text = waveMonsterData.maxHp[0].ToString();
        waveInputs[WaveType.MaxHP][1].text = waveMonsterData.maxHp[1].ToString();
        waveInputs[WaveType.MaxHP][2].text = waveMonsterData.maxHp[2].ToString();

        waveInputs[WaveType.AttackRange][0].text = waveMonsterData.attackRange[0].ToString();
        waveInputs[WaveType.AttackRange][1].text = waveMonsterData.attackRange[1].ToString();
        waveInputs[WaveType.AttackRange][2].text = waveMonsterData.attackRange[2].ToString();

        waveInputs[WaveType.AttackSpeed][0].text = waveMonsterData.attackSpeed[0].ToString();
        waveInputs[WaveType.AttackSpeed][1].text = waveMonsterData.attackSpeed[1].ToString();
        waveInputs[WaveType.AttackSpeed][2].text = waveMonsterData.attackSpeed[2].ToString();

        waveInputs[WaveType.MoveSpeed][0].text = waveMonsterData.moveSpeed[0].ToString();
        waveInputs[WaveType.MoveSpeed][1].text = waveMonsterData.moveSpeed[1].ToString();
        waveInputs[WaveType.MoveSpeed][2].text = waveMonsterData.moveSpeed[2].ToString();
    }

    private void SetUnitInputEvent()
    {
        statInputs[StatType.Attack][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.Attack, str));
        statInputs[StatType.Attack][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.Attack, str));
        statInputs[StatType.Attack][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.Attack, str));

        statInputs[StatType.Defence][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.Defence, str));
        statInputs[StatType.Defence][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.Defence, str));
        statInputs[StatType.Defence][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.Defence, str));

        statInputs[StatType.MaxHP][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.MaxHP, str));
        statInputs[StatType.MaxHP][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.MaxHP, str));
        statInputs[StatType.MaxHP][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.MaxHP, str));

        statInputs[StatType.CriticalPossibility][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.CriticalPossibility, str));
        statInputs[StatType.CriticalPossibility][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.CriticalPossibility, str));
        statInputs[StatType.CriticalPossibility][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.CriticalPossibility, str));

        statInputs[StatType.CriticalMultiplier][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.CriticalMultiplier, str));
        statInputs[StatType.CriticalMultiplier][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.CriticalMultiplier, str));
        statInputs[StatType.CriticalMultiplier][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.CriticalMultiplier, str));

        statInputs[StatType.AttackSpeed][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.AttackSpeed, str));
        statInputs[StatType.AttackSpeed][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.AttackSpeed, str));
        statInputs[StatType.AttackSpeed][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.AttackSpeed, str));

        statInputs[StatType.MoveSpeed][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.MoveSpeed, str));
        statInputs[StatType.MoveSpeed][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.MoveSpeed, str));
        statInputs[StatType.MoveSpeed][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.MoveSpeed, str));

        statInputs[StatType.AttackRange][0].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Tanker, StatType.AttackRange, str));
        statInputs[StatType.AttackRange][1].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Dealer, StatType.AttackRange, str));
        statInputs[StatType.AttackRange][2].onEndEdit.AddListener((str) => UnitCombatPowerCalculator.SetStat(UnitTypes.Healer, StatType.AttackRange, str));
    }

    private void SetMonsterInputEvent()
    {
        waveInputs[WaveType.MonsterCount][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.MonsterCount, str));
        waveInputs[WaveType.MonsterCount][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.MonsterCount, str));
        waveInputs[WaveType.MonsterCount][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.MonsterCount, str));

        waveInputs[WaveType.Attack][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.Attack, str));
        waveInputs[WaveType.Attack][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.Attack, str));
        waveInputs[WaveType.Attack][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.Attack, str));

        waveInputs[WaveType.MaxHP][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.MaxHP, str));
        waveInputs[WaveType.MaxHP][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.MaxHP, str));
        waveInputs[WaveType.MaxHP][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.MaxHP, str));

        waveInputs[WaveType.AttackRange][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.AttackRange, str));
        waveInputs[WaveType.AttackRange][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.AttackRange, str));
        waveInputs[WaveType.AttackRange][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.AttackRange, str));

        waveInputs[WaveType.AttackSpeed][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.AttackSpeed, str));
        waveInputs[WaveType.AttackSpeed][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.AttackSpeed, str));
        waveInputs[WaveType.AttackSpeed][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.AttackSpeed, str));

        waveInputs[WaveType.MoveSpeed][0].onEndEdit.AddListener((str) => SetMonsterStat(0, WaveType.MoveSpeed, str));
        waveInputs[WaveType.MoveSpeed][1].onEndEdit.AddListener((str) => SetMonsterStat(1, WaveType.MoveSpeed, str));
        waveInputs[WaveType.MoveSpeed][2].onEndEdit.AddListener((str) => SetMonsterStat(2, WaveType.MoveSpeed, str));

    }

    private void SetMonsterStat(int index, WaveType waveType, string value)
    {
        switch (waveType)
        {
            case WaveType.MonsterCount:
                if (int.TryParse(value, out int monsterCount))
                {
                    machine.waveMonsterDatas[machine.waveTarget].slots[index] = monsterCount;
                }
                break;
            case WaveType.Attack:
                BigNumber attack = value;
                machine.waveMonsterDatas[machine.waveTarget].attack[index] = attack;
                break;
            case WaveType.MaxHP:
                BigNumber maxHp = value;
                machine.waveMonsterDatas[machine.waveTarget].maxHp[index] = maxHp;
                break;
            case WaveType.AttackRange:
                if (float.TryParse(value, out float range))
                {
                    machine.waveMonsterDatas[machine.waveTarget].attackRange[index] = range;
                }
                break;
            case WaveType.AttackSpeed:
                if (float.TryParse(value, out float attackSpeed))
                {
                    machine.waveMonsterDatas[machine.waveTarget].attackSpeed[index] = attackSpeed;
                }
                break;
            case WaveType.MoveSpeed:
                if (float.TryParse(value, out float speed))
                {
                    machine.waveMonsterDatas[machine.waveTarget].moveSpeed[index] = speed;
                }
                break;
        }
    }
}
