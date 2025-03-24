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

    private static int requiredCount = 5;
    private int currentElementCount = 0;
    private int nextElementCount = 0;
    private int count = 0;
    private void Start()
    {
        minusButton.onClick.AddListener(() => OnClickMinusButton());
        plusButton.onClick.AddListener(() => OnClickPlusButton());
    }
    public void Initialize(InventoryElement currentElement, InventoryElement nextElement)
    {
        var currentElementData = DataTableManager.SoldierTable.GetData(currentElement.soldierId);
        var nextElementData = DataTableManager.SoldierTable.GetData(nextElement.soldierId);

        var currentElementSprite = currentElement.GetComponent<Image>().sprite;
        var nextElementSprite = nextElement.GetComponent<Image>().sprite;

        currentSoldierInfo.Initialize(currentElement.gradeIndex.ToString(), currentElement.Count.ToString(), currentElementSprite);
        nextSoldierInfo.Initialize(nextElement.gradeIndex.ToString(), nextElement.Count.ToString(), nextElementSprite);

        currentElementCount = currentElement.Count;
        nextElementCount = nextElement.Count;

        currentCountText.text = currentElementCount.ToString();
        nextCountText.text = nextElementCount.ToString();

        countText.text = count.ToString();
        ButtonUpdate();
    }

    public void OnClickPlusButton()
    {
        if(currentElementCount < requiredCount)
        {
            return;
        }

        count++;

        countText.text = count.ToString();
        if(count == 0)
        {
            currentCountText.text = $"{currentElementCount}";
            nextCountText.text = $"{nextElementCount}";
        }
        else
        {
            currentCountText.text = $"{currentElementCount}({-count * requiredCount})";
            nextCountText.text = $"{nextElementCount}(+{count})";
        }
        ButtonUpdate();
    }

    public void OnClickMinusButton()
    {
        if (count < 0)
        {
            return;
        }

        count--;

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
        ButtonUpdate();
    }

    public void ButtonUpdate()
    {
        if(currentElementCount > requiredCount)
        {
            plusButton.interactable = true;
        }
        else
        {
            plusButton.interactable = false;
        }

        if(count <= 0)
        {
            minusButton.interactable = false;
        }
        else
        {
            minusButton.interactable = true;
        }
    }
}
