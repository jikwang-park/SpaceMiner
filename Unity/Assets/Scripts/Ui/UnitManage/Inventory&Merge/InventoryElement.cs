using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class InventoryElement : MonoBehaviour
{
    public bool IsLocked { get; private set; } = true;
    public int Count { get; private set; } = 0;
    public int Level { get; private set; } = 0;
    public Grade Grade { get; private set; } = 0;
    public int soldierId; //250331 HKY 데이터형 변경

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
    [SerializeField]
    private AddressableImage icon;

    public Inventory parentInventory;
    private bool onAlarm = false;
    private static readonly Color LockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    private static readonly Color UnlockedColor = Color.white;
    private Image backgroundImage;
    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }
    private void Start()
    {
        backgroundImage.color = IsLocked ? LockedColor : UnlockedColor;
        if(alarmImage != null)
        {
            alarmImage.SetActive(false);
        }
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
        if(backgroundImage != null)
        {
            backgroundImage.color = UnlockedColor;
        }
    }
    public void SetID(int id) //250331 HKY 데이터형 변경
    {
        soldierId = id;
        SetIcon();
    }
    private void SetIcon()
    {
        int spriteId = DataTableManager.SoldierTable.GetData(soldierId).SpriteID;
        icon.SetSprite(spriteId);
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
