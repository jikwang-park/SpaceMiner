using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon2EndWindow : MonoBehaviour
{
    private const string prefabAddress = "Assets/Addressables/Prefabs/UI/Stage/Dungeon/DungeonClearRewardIcon.prefab";

    [SerializeField]
    private TextMeshProUGUI totalDamageText;

    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    [SerializeField]
    private Transform iconParent;

    private List<DungeonClearRewardIcon> rewardIcons = new List<DungeonClearRewardIcon>();

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnDisable()
    {
        for (int i = 0; i < rewardIcons.Count; ++i)
        {
            rewardIcons[i].Release();
        }
        rewardIcons.Clear();
    }

    public void Open(BigNumber damage)
    {
        gameObject.SetActive(true);

        totalDamageText.text = damage.ToString();
    }

    public void Open(BigNumber damage, SortedList<int, BigNumber> rewards)
    {
        Open(damage);
        SoundManager.Instance.PlaySFX("SuccessDungeonSFX");
        foreach (var reward in rewards)
        {
            var itemIconGo = stageManager.StageUiManager.ObjectPoolManager.Get(prefabAddress);
            itemIconGo.transform.SetParent(iconParent);
            var rewardIcon = itemIconGo.GetComponent<DungeonClearRewardIcon>();
            rewardIcons.Add(rewardIcon);
            rewardIcon.SetItem(reward.Key, reward.Value);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        stageManager.SetStatus(IngameStatus.Planet);

        gameObject.SetActive(false);
    }

    public void Retry()
    {
        var curStage = DataTableManager.DungeonTable.GetData(Variables.currentDungeonType, Variables.currentDungeonStage);
        var stageSaveData = SaveLoadManager.Data.stageSaveData;
        if (ItemManager.GetItemAmount(curStage.NeedKeyItemID) < curStage.NeedKeyItemCount)
        {
            requirementWindow.OpenNeedKey();
            return;
        }

        stageManager.ResetStage();
        gameObject.SetActive(false);
    }

    public void MoveToShop()
    {
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetPopUpInactive(3);
        stageManager.SetStatus(IngameStatus.Planet);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetTabActive(3);
        gameObject.SetActive(false);
    }
}
