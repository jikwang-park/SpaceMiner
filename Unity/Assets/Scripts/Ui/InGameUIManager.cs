using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    private const string stageTextFormat = "{0}-{1}";
    private const string waveTextFormat = "{0} Wave";
    private const string dungeonTextFormat = "Dungeon {0}-{1}";

    [SerializeField]
    private TextMeshProUGUI stageText;
    [SerializeField]
    private TextMeshProUGUI waveText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private StageEndWindow stageEndWindow;
    [SerializeField]
    private DungeonEndWindow dungeonEndWindow;

    public void SetTimer(float remainTime)
    {
        timerText.text = remainTime.ToString("F2");
    }

    public void SetStageText(int planet, int stage)
    {
        stageText.text = string.Format(stageTextFormat, planet, stage);
    }

    public void SetWaveText(int wave)
    {
        waveText.text = string.Format(waveTextFormat, wave);
    }

    public void SetDungeonStageText(int dungeonId, int stage)
    {
        stageText.text = string.Format(dungeonTextFormat, dungeonId, stage);
    }

    public void OpenStageEndWindow(string message)
    {
        stageEndWindow.Open(message);
    }

    public void CloseStageEndWindow()
    {
        stageEndWindow.Close();
    }

    public void OpenDungeonEndWindow(string message, bool isCleared)
    {
        dungeonEndWindow.Open(message,isCleared);
    }

    public void CloseDungeonEndWindow()
    {
        dungeonEndWindow.Close();
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
