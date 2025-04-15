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
        gameObject.SetActive(true);
        var itemData = DataTableManager.ItemTable.GetData(data.RewardItemID);
        icon.SetSprite(itemData.SpriteID);
        rewardText.SetStringArguments(data.RewardItemCount.ToString());
    }
}
