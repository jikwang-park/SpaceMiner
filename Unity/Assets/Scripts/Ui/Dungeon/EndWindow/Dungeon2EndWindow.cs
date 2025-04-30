using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dungeon2EndWindow : MonoBehaviour
{
    [SerializeField]
    private LocalizationText messageText;
    [SerializeField]
    private DungeonRewardRow rewardRowPrefab;
    [SerializeField]
    private DungeonRequirementWindow requirementWindow;

    [SerializeField]
    private Transform rowParent;

    private List<DungeonRewardRow> rows = new List<DungeonRewardRow>();

    private StageManager stageManager;

    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
    }

    private void OnDisable()
    {
        for(int i = 0; i< rows.Count;++i)
        {
            Destroy(rows[i].gameObject);
        }
        rows.Clear();
    }

    public void Open(BigNumber damage)
    {
        gameObject.SetActive(true);
        messageText.SetColor(Color.red);
        messageText.SetString(60011, damage.ToString());
    }

    public void Open(BigNumber damage, SortedList<int, BigNumber> rewards)
    {
        gameObject.SetActive(true);

        foreach (var reward in rewards)
        {
            var row = Instantiate(rewardRowPrefab, rowParent);
            row.icon.SetItemSprite(reward.Key);
            rows.Add(row);
            row.text.text = reward.Value.ToString();
        }

        messageText.SetString(60011, damage.ToString());
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
            requirementWindow.Open(DungeonRequirementWindow.Status.KeyCount);
            return;
        }

        stageManager.ResetStage();
        gameObject.SetActive(false);
    }

    public void MoveToShop()
    {
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetPopUpInactive(1);
        stageManager.SetStatus(IngameStatus.Planet);
        stageManager.StageUiManager.UIGroupStatusManager.UiDict[IngameStatus.Planet].SetTabActive(3);
        gameObject.SetActive(false);
    }
}
