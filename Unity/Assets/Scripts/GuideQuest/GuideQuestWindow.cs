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

    private void Awake()
    {
        cleared = false;
        GuideQuestManager.OnQuestProgressChanged += UpdateProgress;
        GuideQuestManager.OnClear += OnQuestClear;
    }

    private void Start()
    {
        questDescriptionText.text = GuideQuestManager.currentQuestData.StringID.ToString();
        UpdateProgress();
        GuideQuestManager.QuestProgressChange(GuideQuestManager.currentQuestData.MissionClearType);
    }

    private void UpdateProgress()
    {
        questDescriptionText.text = GuideQuestManager.currentQuestData.StringID.ToString();
        int monsterCount = SaveLoadManager.Data.questProgressData.monsterCount;
        int goal = GuideQuestManager.currentQuestData.TargetCount;
        int rewardCount = GuideQuestManager.currentQuestData.RewardCount;
        if (!GuideQuestManager.isCleared)
        {
            questProgressText.text = $"{GuideQuestManager.Progress} / {goal}";
        }
        rewardCountText.text = $"{rewardCount}";
    }

    private void OnQuestClear()
    {
        cleared = true;
        questProgressText.text = "Clear";
    }

    public void QuestSuccess()
    {
        if (cleared)
        {
            cleared = false;
            GuideQuestManager.GetReward();
            UpdateProgress();
        }
    }
}
