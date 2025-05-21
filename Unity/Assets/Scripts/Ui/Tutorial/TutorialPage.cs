using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    [SerializeField]
    private LocalizationText titleText;

    [SerializeField]
    private LocalizationText detailText;

    [SerializeField]
    private AddressableImage image;

    [SerializeField]
    private TextMeshProUGUI indexText;

    private int index;

    [SerializeField]
    private Button closeButton;

    private List<TutorialTable.Data> datas;

    private TutorialTable.QuestTypes tutorialType;

    private void Start()
    {
        if (closeButton is null)
        {
            Show(TutorialTable.QuestTypes.FirstPlay);
        }
    }

    public void Show(TutorialTable.QuestTypes type)
    {
        tutorialType = type;
        datas = DataTableManager.TutorialTable.GetDatas(tutorialType);
        index = 0;
        ShowData();

        if (closeButton is not null)
        {
            closeButton.interactable = SaveLoadManager.Data.TutorialOpened[tutorialType];
        }
    }

    public void IndexChange(bool isUp)
    {
        if (isUp && index + 1 < datas.Count)
        {
            ++index;
            ShowData();
            if (!SaveLoadManager.Data.TutorialRewardGot[tutorialType]
                && index == datas.Count - 1)
            {
                SaveLoadManager.Data.TutorialRewardGot[tutorialType] = true;
                SaveLoadManager.Data.TutorialOpened[tutorialType] = true;

                if (closeButton is not null)
                {
                    closeButton.interactable = true;
                }

                if (datas[index].RewardItemID != 0)
                {
                    ItemManager.AddItem(datas[index].RewardItemID, datas[index].RewardItemCount);
                }

                SaveLoadManager.SaveGame();
            }
        }
        else if (!isUp && index > 0)
        {
            --index;
            ShowData();
        }
    }

    private void ShowData()
    {
        titleText.SetString(datas[index].NameStringID);
        image.SetSprite(datas[index].SpriteID);
        indexText.text = $"{index + 1} / {datas.Count}";
        detailText.SetString(datas[index].DetailStringID);
    }
}
