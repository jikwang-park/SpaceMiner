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
    private Button minusButton;
    [SerializeField]
    private Button plusButton;
    [SerializeField]
    private Button mergeButton;
    [SerializeField]
    private Button equipButton;

    private int requiredMergeCount = 0;
    private int currentElementCount = 0;
    private int nextElementCount = 0;
    private int count = 0;
    private int currentElementId;
    private int nextElementId;

    public Action equipAction;
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

        currentSoldierInfo.Initialize(currentElement.Grade, currentElement.Level, "", currentElementSprite);
        nextSoldierInfo.Initialize(nextElement.Grade, nextElement.Level, "", nextElementSprite);

        currentElementCount = currentElement.Count;
        nextElementCount = nextElement.Count;
        count = 0;

        currentCountText.text = currentElementCount.ToString();
        nextCountText.text = nextElementCount.ToString();

        countText.text = count.ToString();
        requiredMergeCount = 5;
        currentElementId = currentElement.soldierId;
        nextElementId = nextElement.soldierId;

        UpdateButton();
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
        equipAction?.Invoke();
    }
}
