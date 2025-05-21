using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideQuestRewardWindow : MonoBehaviour
{
    [SerializeField]
    private LocalizationText rewardText;
    [SerializeField]
    private AddressableImage icon;


    public void Show(GuideQuestTable.Data data)
    {
        //TODO: ��Ʈ�����̺� ������ �߰� �� �ʿ�
        SoundManager.Instance.PlaySFX("QuestCompleteSFX");
        gameObject.SetActive(true);
        icon.SetItemSprite(data.RewardItemID);
        rewardText.SetStringArguments(data.RewardItemCount.ToString());
    }
}
