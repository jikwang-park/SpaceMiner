using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineBattle : MonoBehaviour
{
    [SerializeField]
    private LocalizationText planetNameText;

    [SerializeField]
    private TextMeshProUGUI stageText;

    [SerializeField]
    private AddressableImage rewarditem1Icon;

    [SerializeField]
    private TextMeshProUGUI rewarditem1Text;

    [SerializeField]
    private AddressableImage rewarditem2Icon;

    [SerializeField]
    private TextMeshProUGUI rewarditem2Text;

    [SerializeField]
    private TextMeshProUGUI rewarditem2Probability;

    private List<MiningBattleTable.Data> datas;

    private int planetID;
    private int index;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        this.planetID = Variables.planetMiningID;
        datas = DataTableManager.MiningBattleTable.GetDatas(planetID);
        index = SaveLoadManager.Data.stageSaveData.HighMineStage[this.planetID] - 1;
        planetNameText.SetString(datas[0].NameStringID);
        RefreshText();
    }

    public void ChangeStage(bool isUp)
    {
        if (isUp && index + 1 < datas.Count)
        {
            ++index;
            RefreshText();
        }
        else if (!isUp && index > 0)
        {
            --index;
            RefreshText();
        }
    }

    private void RefreshText()
    {
        stageText.text = datas[index].Stage.ToString();
        rewarditem1Icon.SetItemSprite(datas[index].Reward1ItemID);
        rewarditem1Text.text = datas[index].Reward1ItemCount.ToString();
        rewarditem2Icon.SetItemSprite(datas[index].Reward2ItemID);
        rewarditem2Text.text = datas[index].Reward2ItemCount.ToString();
        rewarditem2Probability.text = datas[index].Reward2ItemProbability.ToString("P2");
    }

    public void OnClickEnter()
    {
        stageManager.MiningBattleStart();
    }
}
