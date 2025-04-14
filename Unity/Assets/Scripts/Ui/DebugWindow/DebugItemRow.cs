using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugItemRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI idText;

    [SerializeField]
    private LocalizationText nameText;

    [SerializeField]
    private TextMeshProUGUI amountText;

    [SerializeField]
    private TMP_InputField amountSetInput;

    private ItemTable.Data data;

    public void Set(int itemId)
    {
        data = DataTableManager.ItemTable.GetData(itemId);
        idText.text = itemId.ToString();
        nameText.SetString(data.NameStringID.ToString());
        Refresh();
    }

    public void Refresh()
    {
        amountText.text = ItemManager.GetItemAmount(data.ID).ToString();
    }

    public void SetAmount()
    {
        if (string.IsNullOrEmpty(amountSetInput.text))
        {
            return;
        }

        try
        {
            BigNumber SetAmount = amountSetInput.text;
            var currentAmount = ItemManager.GetItemAmount(data.ID);
            if (currentAmount > SetAmount)
            {
                ItemManager.ConsumeItem(data.ID, currentAmount - SetAmount);
            }
            else if (currentAmount < SetAmount)
            {
                ItemManager.AddItem(data.ID, SetAmount - currentAmount);
            }
            Refresh();
        }
        catch
        {
            amountSetInput.text = "Cannot Parse";
        }
    }

    public void AddAmount()
    {
        ItemManager.AddItem(data.ID, 1);
        Refresh();
    }

    public void ConsumeAmount()
    {
        ItemManager.ConsumeItem(data.ID, 1);
        Refresh();
    }
}
