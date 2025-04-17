using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class MiningRobotShopElement : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> gradeNameSprites = new List<Sprite>();
    [SerializeField]
    private AddressableImage robotIcon;
    [SerializeField]
    private Image robotGradeImage;
    [SerializeField]
    private AddressableImage itemIcon;
    [SerializeField]
    private LocalizationText decribeText;
    [SerializeField]
    private Button button;

    private int needItemId;
    private string needItemString;
    private int paymentItemId;

    private BigNumber needAmount;
    private int paymentItemAmount;


    public void Initialize(ShopTable.Data data)
    {
        needItemId = data.NeedItemID;
        var needItemStringId = DataTableManager.ItemTable.GetData(needItemId).NameStringID;
        needItemString = DataTableManager.StringTable.GetData(needItemStringId);

        paymentItemId = data.PaymentItemID;

        needAmount = data.NeedItemCount;
        paymentItemAmount = data.PayCount;

        int needItemSpriteId = DataTableManager.ItemTable.GetData(needItemId).SpriteID;
        itemIcon.SetSprite(needItemSpriteId);

        var robotData = DataTableManager.RobotTable.GetData(paymentItemId);
        robotIcon.SetSprite(robotData.SpriteID);

        var robotGrade = robotData.Grade;
        robotGradeImage.sprite = gradeNameSprites[(int)robotGrade - 1];

        UpdateUI();
    }
    private void OnEnable()
    {
        ItemManager.OnItemAmountChanged += DoItemAmountChanged;
    }
    private void OnDisable()
    {
        ItemManager.OnItemAmountChanged -= DoItemAmountChanged;

    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ItemManager.AddItem(needItemId, 100);
        }
    }
#endif
    private void UpdateUI()
    {
        decribeText.SetStringArguments(needItemString.ToString(), needAmount.ToString());
        button.interactable = ItemManager.CanConsume(needItemId, needAmount);
    }
    public void OnClickBuyButton()
    {
        if(ItemManager.CanConsume(needItemId, needAmount))
        {
            ItemManager.ConsumeItem(needItemId, needAmount);
            MiningRobotInventoryManager.AddRobot(paymentItemId);
            UpdateUI();
        }
    }
    private void DoItemAmountChanged(int itemId, BigNumber amount)
    {
        if(needItemId == itemId)
        {
            UpdateUI();
        }
    }
}
