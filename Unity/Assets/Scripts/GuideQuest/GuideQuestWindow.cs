using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideQuestWindow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questDescriptionText;

    [SerializeField]
    private TextMeshProUGUI questProgressText;

    [SerializeField]
    private TextMeshProUGUI rewardCountText;

    [SerializeField]
    private Image questIcon;

    private bool cleared;

    private StageManager stageManager;

    private void Awake()
    {
        cleared = false;
        GuideQuestManager.OnQuestProgressChanged += UpdateProgress;
        GuideQuestManager.OnClear += OnQuestClear;
    }

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        questDescriptionText.text = GuideQuestManager.currentQuestData.StringID.ToString();
        UpdateProgress();
        GuideQuestManager.RefreshQuest();
    }

    private void UpdateProgress()
    {
        if (GuideQuestManager.currentQuestData is null)
        {
            return;
        }

        questDescriptionText.text = GuideQuestManager.currentQuestData.StringID.ToString();
        int monsterCount = SaveLoadManager.Data.questProgressData.monsterCount;
        int goal = GuideQuestManager.currentQuestData.TargetCount;
        int rewardCount = GuideQuestManager.currentQuestData.RewardCount;

        questProgressText.text = $"{GuideQuestManager.Progress} / {goal}";
        rewardCountText.text = $"{rewardCount}";
    }

    private void OnQuestClear()
    {
        cleared = true;
        questProgressText.text = $"{questProgressText.text} Clear";
    }

    public void QuestSuccess()
    {
        if (cleared)
        {
            cleared = false;
            stageManager.StageUiManager.IngameUIManager.GuideQuestRewardWindow.Show(GuideQuestManager.currentQuestData);
            GuideQuestManager.GetReward();
            UpdateProgress();
        }
    }
}
