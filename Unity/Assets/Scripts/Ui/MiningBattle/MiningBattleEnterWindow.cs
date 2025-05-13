using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiningBattleEnterWindow : MonoBehaviour
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
    private TextMeshProUGUI rewarditem2ProbabilityText;

    [SerializeField]
    private LocalizationText remainCountText;

    private List<MiningBattleTable.Data> datas;

    private int planetID;
    private int index;
    private int maxIndex;

    private StageManager stageManager;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        planetID = Variables.planetMiningID;
        datas = DataTableManager.MiningBattleTable.GetDatas(planetID);
        maxIndex = SaveLoadManager.Data.stageSaveData.HighMineStage[planetID] - 1;
        index = maxIndex;
        planetNameText.SetString(DataTableManager.PlanetTable.GetData(datas[0].PlanetTableID).NameStringID);
        RefreshText();
        RefreshCount();
    }

    public void ChangeStage(bool isUp)
    {
        if (isUp && index < maxIndex)
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
        rewarditem2ProbabilityText.text = datas[index].Reward2ItemProbability.ToString("P2");
    }

    private void RefreshCount()
    {
        if (TimeManager.Instance.IsNewDay(SaveLoadManager.Data.mineBattleData.lastClearTime))
        {
            SaveLoadManager.Data.mineBattleData.mineBattleCount = 0;
            SaveLoadManager.Data.mineBattleData.lastClearTime = TimeManager.Instance.GetEstimatedServerTime();
        }

        remainCountText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
    }

    public void OnClickEnter()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            Variables.planetMiningStage = index + 1;
            stageManager.MiningBattleStart();
        }
    }
}
