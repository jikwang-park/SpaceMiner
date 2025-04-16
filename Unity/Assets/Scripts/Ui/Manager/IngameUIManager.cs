using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    private const string stageTextFormat = "{0}-{1}";
    private const string waveTextFormat = "{0} Wave";
    private const string dungeonTextFormat = "Dungeon {0}-{1}";

    [SerializeField]
    private TextMeshProUGUI stageText;
    [field: SerializeField]
    public TextMeshProUGUI waveText { get; private set; }
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private StageEndWindow stageEndWindow;
    [SerializeField]
    private DungeonEndWindow dungeonEndWindow;
    [field: SerializeField]
    public StageSelectWindow StageSelectWindow { get; private set; }
    [field: SerializeField]
    public GuideQuestRewardWindow GuideQuestRewardWindow { get; private set; }

    [SerializeField]
    private SerializedDictionary<IngameStatus, List<GameObject>> statusObjectLists;

    private IngameStatus ingameStatus;

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

    public void OpenStageEndWindow(string message, float duration)
    {
        stageEndWindow.Open(message,duration);
    }

    public void CloseStageEndWindow()
    {
        stageEndWindow.Close();
    }

    public void OpenDungeonEndWindow(bool isCleared, bool firstCleared)
    {
        dungeonEndWindow.Open(isCleared, firstCleared);
    }

    public void CloseDungeonEndWindow()
    {
        dungeonEndWindow.Close();
    }

    public void SetGoldText()
    {
        goldText.text = $"{ItemManager.GetItemAmount((int)Currency.Gold)}G";
    }

    public void SetStatus(IngameStatus status)
    {
        if (ingameStatus == status)
        {
            return;
        }

        if (statusObjectLists.ContainsKey(ingameStatus))
        {
            foreach (var gameobject in statusObjectLists[ingameStatus])
            {
                gameobject.SetActive(false);
            }
        }

        if (statusObjectLists.ContainsKey(status))
        {
            foreach (var gameobject in statusObjectLists[status])
            {
                gameobject.SetActive(true);
            }
        }
        ingameStatus = status;
    }
}
