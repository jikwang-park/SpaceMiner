using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaPurchaseUI : MonoBehaviour
{
    [SerializeField]
    private GachaResultPanelUI gachaResultPanelUI;
    [SerializeField]
    private GachaPurchaseButton gachaOneButton;
    [SerializeField] 
    private GachaPurchaseButton gachaRepeatButton;
    [SerializeField]
    private GachaPurchaseButton gachaRepeat2Button;

    private int currentGachaId;
    private void Awake()
    {
        gachaOneButton.onClickGachaButton += DoGacha;
        gachaRepeatButton.onClickGachaButton += DoGacha;
        gachaRepeat2Button.onClickGachaButton += DoGacha;
    }
    public void Initialize(GachaTable.Data data)
    {
        currentGachaId = data.ID;
        if(GachaManager.useTicket)
        {
            gachaOneButton.Initialize(1, 1, data.NeedItemID2);
            gachaRepeatButton.Initialize(data.RepeatCount1, data.RepeatCount1, data.NeedItemID2);
            gachaRepeat2Button.Initialize(data.RepeatCount2, data.RepeatCount2, data.NeedItemID2);
        }
        else
        {
            gachaOneButton.Initialize(1, GachaManager.CalCulateCost(currentGachaId, 1), data.NeedItemID1);
            gachaRepeatButton.Initialize(data.RepeatCount1, GachaManager.CalCulateCost(currentGachaId, data.RepeatCount1), data.NeedItemID1);
            gachaRepeat2Button.Initialize(data.RepeatCount2, GachaManager.CalCulateCost(currentGachaId, data.RepeatCount2), data.NeedItemID1);
        }

    }
    private void OnEnable()
    {
        GachaManager.onUseTicketChanged += DoUseTicketChanged;
        if(currentGachaId != default)
        {
            Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
        }
    }
    private void OnDisable()
    {
        GachaManager.onUseTicketChanged -= DoUseTicketChanged;
    }
    public void ToggleUseTicket()
    {
        GachaManager.ToggleUseTicket();
    }
    private void DoUseTicketChanged()
    {
        Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
    }
    private void DoGacha(int count)
    {
        var gachaResults = GachaManager.Gacha(currentGachaId, count, GachaManager.useTicket);
        if(gachaResults != null)
        {
            InventoryManager.Add(gachaResults);
            if(!gachaResultPanelUI.gameObject.activeSelf)
            {
                gachaResultPanelUI.gameObject.SetActive(true);
            }
            gachaResultPanelUI.SetResult(gachaResults, currentGachaId);
            Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
        }
    }
}
