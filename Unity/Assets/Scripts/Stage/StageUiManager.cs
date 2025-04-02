using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class StageUiManager : MonoBehaviour
{
    private const string stageTextFormat = "{0}-{1}\n{2} Wave";
    private const string dungeonTextFormat = "Dungeon {0}-{1}\n{2} Wave";
    private const string fail = "Fail";
    private const string clear = "Clear";

    [SerializeField]
    private TextMeshProUGUI stageText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private GameObject stageEndMessageWindow;
    [SerializeField]
    private TextMeshProUGUI stageEndMessageText;
    [SerializeField]
    private StageSelectWindow stageSelectWindow;

    public ObjectPoolManager objectPoolManager { get; private set; }

    private void Awake()
    {
        objectPoolManager = GetComponent<ObjectPoolManager>();
    }

    public void SetTimer(float remainTime)
    {
        timerText.text = remainTime.ToString("F2");
    }

    public void SetStageText(int planet, int stage, int wave)
    {
        stageText.text = string.Format(stageTextFormat, planet, stage, wave);
    }

    public void SetStageTextDungeon(int dungeonId, int stage, int wave)
    {
        stageText.text = string.Format(dungeonTextFormat, dungeonId, stage, wave);
    }

    public void SetStageMessage(bool isCleared)
    {
        if (isCleared)
        {
            stageEndMessageText.text = clear;
        }
        else
        {
            stageEndMessageText.text = fail;
        }
    }

    public void SetActiveStageMessage(bool isActive)
    {
        stageEndMessageWindow.SetActive(isActive);
    }

    public void UnlockStage(int planet, int stage)
    {
        stageSelectWindow.UnlockStage(planet, stage);
    }

    public void SetGoldText()
    {
        goldText.text = $"{ItemManager.GetItemAmount((int)Currency.Gold)}G";
    }

    public void SetIngameStatus(IngameStatus status)
    {
        switch (status)
        {
            case IngameStatus.Planet:
                goldText.gameObject.SetActive(true);
                break;
            case IngameStatus.Dungeon:
                goldText.gameObject.SetActive(false);
                break;
        }
    }
}
