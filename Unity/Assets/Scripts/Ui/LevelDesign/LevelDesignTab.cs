using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesignTab : MonoBehaviour
{
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


    private float exitButtonTime;


    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        machine = (LevelDesignStageStatusMachine)stageManager.GetStage(IngameStatus.LevelDesign);
        exitButtonTime = float.MinValue;
        RefreshText();
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
        }
        if (!isUp && machine.waveLength > 1)
        {
            --machine.waveLength;
            if (machine.waveTarget >= machine.waveLength)
            {
                --machine.waveTarget;
            }
            RefreshText();
        }
    }

    public void WaveTargetUpDown(bool isUp)
    {
        if (isUp && machine.waveTarget + 1 < machine.waveLength)
        {
            ++machine.waveTarget;
            RefreshText();
        }
        if (!isUp && machine.waveTarget > 0)
        {
            --machine.waveTarget;
            RefreshText();
        }
    }

    private void RefreshText()
    {
        spawnDistanceInput.text = machine.respawnDistance.ToString("F0");
        weightInput.text = machine.weight.ToString("F2");
        waveText.text = $"{machine.waveTarget + 1} / {machine.waveLength}";
        timeLimitInput.text = machine.stageTime.ToString("F0");
    }
}
