using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    private bool isLocked = true;
    private string soldierId;
    private int count = 0;
    private int level = 0;

    [SerializeField]
    private Image lockImage;
    [SerializeField]
    private Image selectedImage;
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
            button.interactable = !isLocked;
        }
    }
    private void Start()
    {
        button.onClick.AddListener(() => OnElementClicked());
    }
    public void UnlockElement()
    {
        isLocked = false;
        if (lockImage != null)
        {
            lockImage.gameObject.SetActive(false);
        }
        if (button != null)
        {
            button.interactable = true;
        }
    }
    public void SetID(string id)
    {
        soldierId = id;
    }
    public void SetGrade(int grade)
    {
        if (gradeText != null)
        {
            gradeText.text = $"{grade} µî±Þ";
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        if (levelText != null)
        {
            gradeText.text = "Lv. " + level.ToString();
        }
    }

    public void UpdateCount(int newCount)
    {
        count = newCount;
        if (countText != null)
        {
            countText.text = count.ToString();
        }
    }
    public void OnElementClicked()
    {
        if (parentInventory != null)
        {
            parentInventory.OnElementSelected(this);
        }
    }
    public void Select()
    {
        selectedImage.gameObject.SetActive(true);
    }
    public void Deselect()
    {
        selectedImage.gameObject.SetActive(false);
    }
}
