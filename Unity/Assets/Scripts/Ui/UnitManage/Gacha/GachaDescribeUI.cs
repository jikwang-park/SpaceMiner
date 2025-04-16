using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaDescribeUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI describeText;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void Initialize(GachaTable.Data data, Sprite backgroundSprite)
    {
        describeText.text = DataTableManager.StringTable.GetData(data.DetailStringID); //250331 HKY 데이터형 변경
        image.sprite = backgroundSprite;
    }
}
