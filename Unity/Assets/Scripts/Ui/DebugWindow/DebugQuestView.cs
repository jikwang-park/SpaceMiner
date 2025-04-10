using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugQuestView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI idText;
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private TextMeshProUGUI missionTypeText;
    [SerializeField]
    private TextMeshProUGUI targetText;
    [SerializeField]
    private TextMeshProUGUI progressText;
    [SerializeField]
    private TextMeshProUGUI goalText;

    [SerializeField]
    private TMP_InputField changeInput;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        GuideQuestManager.RefreshQuest();
        var questData = GuideQuestManager.currentQuestData;
        idText.text = questData.ID.ToString();
        turnText.text = questData.Turn.ToString();
        missionTypeText.text = questData.MissionClearType.ToString();
        targetText.text = questData.Target.ToString();
        progressText.text = GuideQuestManager.Progress.ToString();
        goalText.text = questData.TargetCount.ToString();
    }

    public void ChangeQuest()
    {
        var str = changeInput.text;
        changeInput.text = string.Empty;
        if (!int.TryParse(str, out int result))
        {
            return;
        }
        var dict = DataTableManager.GuideQuestTable.orderDict;
        if (!dict.ContainsKey(result))
        {
            return;
        }
        GuideQuestManager.ChangeQuest(result);
        Refresh();
    }
}
