using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInfoImage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI countText;
    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(string grade, string count, Sprite sprite)
    {
        gradeText.text = $"Grade {grade}";
        countText.text = count;
        image.sprite = sprite;
    }
}
