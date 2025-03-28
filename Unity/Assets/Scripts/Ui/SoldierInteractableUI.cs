using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierInteractableUI : MonoBehaviour
{
    [SerializeField]
    private Button attributeButton;
    [SerializeField]
    private Button MergeButton;
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

    private static int requiredCount = 5;
    private int currentElementCount = 0;
    private int nextElementCount = 0;
    private int count = 0;

    public Action<int> mergeAction;
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

        currentSoldierInfo.Initialize(currentElement.GradeIndex.ToString(), currentElement.Count.ToString(), currentElementSprite);
        nextSoldierInfo.Initialize(nextElement.GradeIndex.ToString(), nextElement.Count.ToString(), nextElementSprite);

        currentElementCount = currentElement.Count;
        nextElementCount = nextElement.Count;
        count = 0;

        currentCountText.text = currentElementCount.ToString();
        nextCountText.text = nextElementCount.ToString();

        countText.text = count.ToString();
        UpdateButton();
    }

    public void OnClickPlusButton()
    {
        if(currentElementCount < requiredCount)
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
        if(currentElementCount >= requiredCount * (count + 1))
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
            currentCountText.text = $"{currentElementCount}({-count * requiredCount})";
            nextCountText.text = $"{nextElementCount}(+{count})";
        }
    }
    public void OnClickMergeButton()
    {
        mergeAction?.Invoke(count);
        currentElementCount -= count * requiredCount;
        nextElementCount += count;
        count = 0;

        UpdateCountText();
        UpdateButton();
        currentSoldierInfo.SetCountText(currentCountText.text);
        nextSoldierInfo.SetCountText(nextCountText.text);
    }

    public void OnClickEquipButton()
    {
        equipAction?.Invoke();
    }
}
