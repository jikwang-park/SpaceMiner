using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private List<TutorialTable.Data> datas;

    private void Start()
    {
        Show(1);
    }

    public void Show(int tutorialType)
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
