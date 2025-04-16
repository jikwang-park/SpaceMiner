using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInfoImage : MonoBehaviour
{
    [SerializeField]
    private LocalizationText gradeText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI countText;
    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(Grade grade, int level, string count, Sprite sprite)
    {
        int stringId = DataTableManager.DefaultDataTable.GetID(grade.ToString() + "StringID");
        gradeText.SetString(stringId);
        levelText.text = $"Lv. {level}";
        countText.text = count;
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = sprite;
    }
    public void SetCountText(string countText)
    {
        this.countText.text = countText;
    }
}
