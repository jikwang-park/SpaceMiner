using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInteractableUI : MonoBehaviour
{
    [SerializeField]
    private SoldierInfoImage currentSoldierInfo;
    [SerializeField]
    private SoldierInfoImage nextSoldierInfo;
    [SerializeField]
    private TextMeshProUGUI currentCountText;
    [SerializeField]
    private TextMeshProUGUI nextCountText;
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private LocalizationText describeText;
    [SerializeField]
    private Button minusButton;
    [SerializeField]
    private Button plusButton;
    [SerializeField]
    private Button mergeButton;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Sprite canEquipSprite;
    [SerializeField]
    private Sprite cannotEquipSprite;

    private Image equipButtonImage;

    private int requiredMergeCount = 0;
    private int currentElementCount = 0;
    private int nextElementCount = 0;
    private int count = 0;
    private int currentElementId;
    private int nextElementId;
    private UnitTypes currentType;
    public Action equipAction;
    private void Awake()
    {
        equipButtonImage = equipButton.GetComponent<Image>();
    }
    private void Start()
    {
        minusButton.onClick.AddListener(() => OnClickMinusButton());
        plusButton.onClick.AddListener(() => OnClickPlusButton());
        mergeButton.onClick.AddListener(() => OnClickMergeButton());
        equipButton.onClick.AddListener(() => OnClickEquipButton());
    }
    public void Initialize(InventoryElement currentElement, InventoryElement nextElement)
    {
        var currentElementSprite = currentElement.GetComponent<Image>().sprite;
        var nextElementSprite = nextElement.GetComponent<Image>().sprite;
        
        currentElementId = currentElement.soldierId;
        nextElementId = nextElement.soldierId;

        var currentData = DataTableManager.SoldierTable.GetData(currentElementId);
        var nextData = DataTableManager.SoldierTable.GetData(nextElementId);

        currentSoldierInfo.Initialize(currentElement.Grade, currentElement.Level, "", currentElementSprite, currentData.SpriteID);
        nextSoldierInfo.Initialize(nextElement.Grade, nextElement.Level, "", nextElementSprite, nextData.SpriteID);

        currentElementCount = currentElement.Count;
        nextElementCount = nextElement.Count;
        count = 0;

        currentCountText.text = currentElementCount.ToString();
        nextCountText.text = nextElementCount.ToString();

        countText.text = count.ToString();
        requiredMergeCount = 5;

        currentType = currentData.UnitType;

        int equipId = SaveLoadManager.Data.soldierInventorySaveData[currentType].equipElementID;

        equipButtonImage.sprite = (!currentElement.IsLocked && currentElementId != equipId) ? canEquipSprite : cannotEquipSprite;

        UpdateButton();
        UpdateCountText();
    }

    public void OnClickPlusButton()
    {
        if(currentElementCount < requiredMergeCount)
        {
            return;
        }

        count++;

        UpdateCountText();
        UpdateButton();
    }

    public void OnClickMinusButton()
    {
        if (count < 0)
        {
            return;
        }

        count--;

        UpdateCountText();
        UpdateButton();
    }

    public void UpdateButton()
    {
        if(currentElementCount >= requiredMergeCount * (count + 1))
        {
            plusButton.interactable = true;
        }
        else
        {
            plusButton.interactable = false;
        }

        if(count <= 0)
        {
            mergeButton.interactable = false;
            minusButton.interactable = false;
        }
        else
        {
            mergeButton.interactable = true;
            minusButton.interactable = true;
        }

        if(currentElementId == nextElementId)
        {
            mergeButton.interactable = false;
        }
    }
    private void UpdateCountText()
    {
        countText.text = count.ToString();
        if (count == 0)
        {
            currentCountText.text = $"{currentElementCount}";
            nextCountText.text = $"{nextElementCount}";
        }
        else
        {
            currentCountText.text = $"{currentElementCount}({-count * requiredMergeCount})";
            nextCountText.text = $"{nextElementCount}(+{count})";
        }
        describeText.SetStringArguments((count * requiredMergeCount).ToString(), count.ToString());
    }
    public void OnClickMergeButton()
    {
        if(currentElementId != nextElementId)
        {
            InventoryManager.Merge(currentElementId, count);
            currentElementCount -= count * requiredMergeCount;
            nextElementCount += count;
            count = 0;

            UpdateCountText();
            UpdateButton();
        }
    }

    public void OnClickEquipButton()
    {
        var canEquip = !InventoryManager.GetSoldierData(currentType, currentElementId).isLocked;
        if(canEquip)
        {
            equipAction?.Invoke();
            equipButtonImage.sprite = cannotEquipSprite;
        }
    }
}
