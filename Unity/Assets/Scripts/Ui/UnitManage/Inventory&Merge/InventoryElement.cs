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
    public Grade Grade { get; private set; } = 0;
    public int soldierId; //250331 HKY �������� ����

    [SerializeField]
    private Image lockImage;
    [SerializeField]
    private Image equipImage;
    [SerializeField]
    private LocalizationText gradeText;
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject alarmImage;

    public Inventory parentInventory;
    private Button button;
    private bool onAlarm = false;
    void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        if (button != null)
        {
            button.interactable = !IsLocked;
        }
        if(alarmImage != null)
        {
            alarmImage.SetActive(false);
        }
        button.onClick.AddListener(() => OnElementClicked());
    }
    public void UnlockElement()
    {
        IsLocked = false;
        onAlarm = true;
        if(alarmImage != null)
        {
            alarmImage.SetActive(true);
        }
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(false);
        }
        if (button != null)
        {
            button.interactable = true;
        }
    }
    public void SetID(int id) //250331 HKY �������� ����
    {
        soldierId = id;
    }
    public void SetGrade(Grade grade)
    {
        this.Grade = grade;
        if(gradeText != null)
        {
            int stringId = DataTableManager.DefaultDataTable.GetID(Grade.ToString() + "StringID");
            gradeText.SetString(stringId);
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
    private void OnDisable()
    {
        onAlarm = false;
        alarmImage.gameObject.SetActive(false);
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
