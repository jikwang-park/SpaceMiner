using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningBattleResultWindow : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private LocalizationText stageText;

    [SerializeField]
    private LocalizationText remainingText;

    [SerializeField]
    private LocalizationText clearMessageText;

    [SerializeField]
    private LocalizationText waitTimeText;

    [SerializeField]
    private GameObject clearView;

    [SerializeField]
    private GameObject defeatView;

    [SerializeField]
    private Transform rewardRow;

    [SerializeField]
    private Button nextStageButton;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    private StageManager stageManager;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private float endTime;

    private void Update()
    {
        if (endTime < Time.time)
        {
            gameObject.SetActive(false);
        }
        waitTimeText.SetStringArguments((endTime - Time.time).ToString("F0"));
    }


    private void OnDisable()
    {
        stageManager.StageUiManager.curtain.SetFade(true);

        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
        SoundManager.Instance.PlayBGM("MiningBGM");
    }

    public void ShowClear(MiningBattleTable.Data data, List<(int itemid, BigNumber amount)> gainItem)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX("DungeonSuccessSFX");
        stageText.SetStringArguments(data.Stage.ToString());
        gameObject.SetActive(true);
        clearView.SetActive(true);
        defeatView.SetActive(false);
        remainingText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
        clearMessageText.SetString(Defines.PlanetStageClearStringID);

        if (stageManager is null)
        {
            stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        }

        foreach (var reward in gainItem)
        {
            var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
            itemIconGo.transform.SetParent(rewardRow);
            var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
            rewardIcons.Add(rewardIcon);
            rewardIcon.SetItem(reward.itemid, reward.amount);
        }
        var datas = DataTableManager.MiningBattleTable.GetDatas(Variables.planetMiningID);

        nextStageButton.interactable = Variables.planetMiningStage + 1 <= datas[datas.Count - 1].Stage;

        endTime = Time.time + Defines.MiningBattleResultWait;
    }

    public void ShowDefeat(MiningBattleTable.Data data)
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX("DungeonFailSFX");
        stageText.SetStringArguments(data.Stage.ToString());
        gameObject.SetActive(true);
        defeatView.SetActive(true);
        clearView.SetActive(false);
        remainingText.SetStringArguments(SaveLoadManager.Data.mineBattleData.mineBattleCount.ToString());
        clearMessageText.SetString(Defines.StageFailStringID);
        endTime = Time.time + Defines.MiningBattleResultWait;
    }

    public void NextStage()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            var datas = DataTableManager.MiningBattleTable.GetDatas(Variables.planetMiningID);
            Variables.planetMiningStage = Mathf.Min(Variables.planetMiningStage + 1, datas[datas.Count - 1].Stage);
            stageManager.MiningBattleStart();
        }
        else
        {
            requirementWindow.OpenMiningFullCount();
        }
    }

    public void RetryStage()
    {
        if (SaveLoadManager.Data.mineBattleData.mineBattleCount < Defines.MiningBattleMaxCount)
        {
            gameObject.SetActive(false);
            stageManager.MiningBattleStart();
        }
        else
        {
            requirementWindow.OpenMiningFullCount();
        }
    }
}
