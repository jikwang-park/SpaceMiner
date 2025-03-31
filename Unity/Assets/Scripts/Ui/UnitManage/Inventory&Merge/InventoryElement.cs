using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventoryElement : MonoBehaviour
{
    public bool IsLocked { get; private set; } = true;
    public int Count { get; private set; } = 0;
    public int Level { get; private set; } = 0;
    public int GradeIndex { get; private set; } = 0;
    public int soldierId; //250331 HKY 데이터형 변경

    [SerializeField]
    private Image lockImage;
    [SerializeField]
    private Image equipImage;
    [SerializeField]
    private TextMeshProUGUI gradeText;
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private TextMeshProUGUI levelText;

    public Inventory parentInventory;
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.interactable = !IsLocked;
        }
    }
    private void Start()
    {
        button.onClick.AddListener(() => OnElementClicked());
    }
    public void UnlockElement()
    {
        IsLocked = false;
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(false);
        }
        if (button != null)
        {
            button.interactable = true;
        }
    }
    public void SetID(int id) //250331 HKY 데이터형 변경
    {
        soldierId = id;
    }
    public void SetGrade(int grade)
    {
        GradeIndex = grade;
        if (gradeText != null)
        {
            gradeText.text = $"{grade} Grade";
        }
    }

    public void SetLevel(int level)
    {
        this.Level = level;
        if (levelText != null)
        {
            levelText.text = "Lv. " + level.ToString();
        }
    }

    public void UpdateCount(int newCount)
    {
        Count = newCount;
        if (countText != null)
        {
            countText.text = Count.ToString();
        }
    }
    public void OnElementClicked()
    {
        if (parentInventory != null)
        {
            parentInventory.OnElementSelected(this);
        }
    }
    public void SetEquip()
    {
        equipImage.gameObject.SetActive(true);
    }
    public void SetUnEquip()
    {
        equipImage.gameObject.SetActive(false);
    }
    public void Select()
    {
        
    }

}
