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

    private bool useTicket = false;
    private int currentGachaId;
    private void Awake()
    {
        gachaOneButton.onClickGachaButton += DoGacha;
        gachaRepeatButton.onClickGachaButton += DoGacha;
        gachaRepeat2Button.onClickGachaButton += DoGacha;
    }
    public void Initialize(GachaTable.Data data)
    {
        currentGachaId = data.gachaID;
        if(useTicket)
        {
            gachaOneButton.Initialize(1, 1);
            gachaRepeatButton.Initialize(data.repeat, data.repeat);
            gachaRepeat2Button.Initialize(data.repeat2, data.repeat2);
        }
        else
        {
            gachaOneButton.Initialize(1, GachaManager.CalCulateCost(currentGachaId, 1));
            gachaRepeatButton.Initialize(data.repeat, GachaManager.CalCulateCost(currentGachaId, data.repeat));
            gachaRepeat2Button.Initialize(data.repeat2, GachaManager.CalCulateCost(currentGachaId, data.repeat2));
        }

    }
    public void ToggleUseTicket()
    {
        useTicket = !useTicket;
        Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
    }
    private void DoGacha(int count)
    {
        ItemManager.AddItem(1002, 30); // 250404 SHG - 가챠 테스트용 재화 추가 
        var gachaResults = GachaManager.Gacha(currentGachaId, count, useTicket);
        if(gachaResults != null)
        {
            InventoryManager.Add(gachaResults);
            gachaResultPanelUI.gameObject.SetActive(true);
            gachaResultPanelUI.SetResult(gachaResults, currentGachaId);
            Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
        }
    }
}
