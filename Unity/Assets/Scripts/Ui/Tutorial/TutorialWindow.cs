using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
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

    [SerializeField]
    private Button backgroundButton;

    private List<TutorialTable.Data> datas;

    public void Show(TutorialTable.QuestTypes tutorialType)
    {
        datas = DataTableManager.TutorialTable.GetDatas(tutorialType);
        index = 0;
        ShowData();
    }

    public void IndexChange(bool isUp)
    {
        if (isUp && index + 1 < datas.Count)
        {
            ++index;
            ShowData();
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
        indexText.text = (index + 1).ToString();
        detailText.SetString(datas[index].DetailStringID);
    }
}
