using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaPurchaseUI : MonoBehaviour
{
    [SerializeField]
    private InventoryPanelUI inventoryPanelUI;
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
        currentGachaId = data.gachaID;
        gachaOneButton.Initialize(1, GachaManager.CalCulateCost(currentGachaId, 1));
        gachaRepeatButton.Initialize(data.repeat, GachaManager.CalCulateCost(currentGachaId, data.repeat));
        gachaRepeat2Button.Initialize(data.repeat2, GachaManager.CalCulateCost(currentGachaId, data.repeat2));
    }

    private void DoGacha(int count)
    {
        ItemManager.AddItem((int)Currency.Gold, 100000000);
        var gachaResults = GachaManager.Gacha(currentGachaId, count);
        if(gachaResults != null)
        {
            inventoryPanelUI.ApplyGachaToInventory(gachaResults);
            gachaResultPanelUI.gameObject.SetActive(true);
            gachaResultPanelUI.SetResult(gachaResults, currentGachaId);
            Initialize(DataTableManager.GachaTable.GetData(currentGachaId));
        }
    }
}
