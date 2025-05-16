using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningBattleEnterWindow : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private LocalizationText planetNameText;

    [SerializeField]
    private TextMeshProUGUI stageText;

    [SerializeField]
    private Transform iconParent;

    [SerializeField]
    private LocalizationText remainCountText;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    [SerializeField]
    private MiningBattleExterminateWindow exterminateWindow;

    private List<MiningBattleTable.Data> datas;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private int planetID;
    private int index;
    private int maxIndex;
    private int maxClearedIndex;

    private StageManager stageManager;

    [SerializeField]
    private Button exterminateButton;

    private void Start()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        exterminateWindow.OnExterminate += OnExterminate;
    }


    private void OnDisable()
    {
        ClearIcons();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        planetID = Variables.planetMiningID;
        datas = DataTableManager.MiningBattleTable.GetDatas(planetID);
        maxIndex = SaveLoadManager.Data.stageSaveData.HighMineStage[planetID] - 1;
        maxClearedIndex = SaveLoadManager.Data.stageSaveData.ClearedMineStage[planetID] - 1;
        index = maxIndex;
        planetNameText.SetString(DataTableManager.PlanetTable.GetData(datas[0].PlanetTableID).NameStringID);
        RefreshText();
        RefreshCount();
        exterminateButton.interactable = SaveLoadManager.Data.stageSaveData.HighMineStage[planetID] == SaveLoadManager.Data.stageSaveData.ClearedMineStage[planetID];
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
        exterminateButton.interactable = index <= maxClearedIndex;
    }

    private void RefreshText()
    {
        stageText.text = datas[index].Stage.ToString();

        ClearIcons();

        AddIcon(datas[index].Reward1ItemID, datas[index].Reward1ItemCount);

        for (int i = 0; i < datas[index].Reward2ItemIDs.Length; ++i)
        {
            AddIcon(datas[index].Reward2ItemIDs[i], 0, datas[index].Reward2ItemCounts[i]);
        }
    }

    private void AddIcon(int itemID, BigNumber amount)
    {
        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }

        var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
        itemIconGo.transform.SetParent(iconParent);
        var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
        rewardIcons.Add(rewardIcon);
        rewardIcon.SetItem(itemID, amount);
    }

    private void AddIcon(int itemID, BigNumber minAmount, BigNumber maxAmount)
    {
        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }

        var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
        itemIconGo.transform.SetParent(iconParent);
        var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
        rewardIcons.Add(rewardIcon);
        rewardIcon.SetItem(itemID, minAmount, maxAmount);
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

    private void ClearIcons()
    {
        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
    }

    public void OnClickEnter()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            Variables.planetMiningStage = index + 1;
            stageManager.MiningBattleStart();
        }
        else
        {
            requirementWindow.OpenMiningFullCount();
        }
    }

    private void OnExterminate()
    {
        remainCountText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
    }

    public void OpenExterminate()
    {
        exterminateWindow.Open(datas[index]);
    }
}
