using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideQuestRewardWindow : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI titleText { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI rewardText { get; private set; }

    [field: SerializeField]
    public TextMeshProUGUI noticeText { get; private set; }

    public void Show(GuideQuestTable.Data data)
    {
        //TODO: 스트링테이블 데이터 추가 후 필요
        gameObject.SetActive(true);
        titleText.text = $"Quest {data.ID} Complete";
        rewardText.text = $"Reward {data.RewardItemID} {data.RewardItemCount} Got";
        noticeText.text = $"Press Any To Close";
    }
}
