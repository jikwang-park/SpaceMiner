using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIManager : MonoBehaviour
{
    public enum ActionType
    {
        AllSaveData,
        StatReset,
        SoldierReset,
        SoldierUnlock,
        SkillReset,
        BuildingReset,
        RobotReset,
        ShopRestock,
        Time,
        StageReset,
        StageUnlock,
        ItemReset,
    }

    [SerializeField]
    private DebugItemRow itemRowPrefab;

    [SerializeField]
    private Transform itemViewContent;

    [SerializeField]
    private GameObject confirmWindow;

    [SerializeField]
    private TextMeshProUGUI timeModeText;

    [SerializeField]
    private TextMeshProUGUI confirmMessageText;

    private List<DebugItemRow> itemRows = new List<DebugItemRow>();

    private HashSet<int> itemids = new HashSet<int>();

    private Coroutine coRowSet;

    private System.Action confirmAction;

    private TimeManager timeManager;
    private StageManager stageManager;

    private void Start()
    {
        coRowSet = StartCoroutine(CoRowActive());
        timeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        stageManager = timeManager.GetComponent<StageManager>();
        if (timeManager.isDebug)
        {
            timeModeText.text = "����\n�ð�";
        }
        else
        {
            timeModeText.text = "���ͳ�\n�ð�";
        }
    }

    public void RefreshItem()
    {
        foreach (var row in itemRows)
        {
            row.Refresh();
        }
    }

    private void OnEnable()
    {
        if (coRowSet is not null)
        {
            StartCoroutine(CoRowActive());
        }
    }

    private void OnDisable()
    {
        foreach (var row in itemRows)
        {
            row.gameObject.SetActive(false);
        }
    }

    private IEnumerator CoRowActive()
    {
        int count = 0;
        foreach (var row in itemRows)
        {
            row.gameObject.SetActive(true);
            if (++count == 5)
            {
                count = 0;
                yield return null;
            }
        }
        var ids = DataTableManager.ItemTable.GetIds();
        if (ids.Count == itemRows.Count)
        {
            yield break;
        }
        count = 0;
        for (int i = 0; i < ids.Count; ++i)
        {
            if (itemids.Contains(ids[i]))
            {
                continue;
            }

            var row = Instantiate(itemRowPrefab, itemViewContent);
            row.Set(ids[i]);
            itemRows.Add(row);
            if (++count == 5)
            {
                count = 0;
                yield return null;
            }
        }
    }

    public void ConfirmAction()
    {
        confirmAction?.Invoke();
    }

    public void ClearConfirmAction()
    {
        confirmAction = null;
    }

    public void ShowConfirm(int id)
    {
        var type = (ActionType)id;
        switch (type)
        {
            case ActionType.AllSaveData:
                confirmMessageText.text = "���̺� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.SetDefaultData();
                    GuideQuestManager.ChangeQuest(SaveLoadManager.Data.questProgressData.currentQuest);
                };
                break;
            case ActionType.StatReset:
                confirmMessageText.text = "���� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetStatUpgradeData();
                };
                break;
            case ActionType.SoldierReset:
                confirmMessageText.text = "���� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetSoldierInventoryData();
                };
                break;
            case ActionType.SoldierUnlock:
                confirmMessageText.text = "���� ��ü�� �ر��մϴ�";
                confirmAction = () =>
                {
                    InventoryManager.UnlockAllDatas();
                };
                break;
            case ActionType.SkillReset:
                confirmMessageText.text = "��ų ���׷��̵� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetSkillUpgradeData();
                };
                break;
            case ActionType.BuildingReset:
                confirmMessageText.text = "�ǹ� ���׷��̵� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetBuildingUpgradeData();
                };
                break;
            case ActionType.RobotReset:
                confirmMessageText.text = "�κ� ���� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetMiningRobotInventoryData();
                };
                break;
            case ActionType.ShopRestock:
                confirmMessageText.text = "���� ���ż����� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetDungeonKeyShopData();
                };
                break;
            case ActionType.Time:
                confirmMessageText.text = "�ð� ������ �����մϴ�";
                confirmAction = () =>
                {
                    timeManager.ToggleDebugMode();
                    if(timeManager.isDebug)
                    {
                        timeModeText.text = "����\n�ð�";
                    }
                    else
                    {
                        timeModeText.text = "���ͳ�\n�ð�";
                    }
                };
                break;
            case ActionType.StageReset:
                confirmMessageText.text = "�������� �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetStageSaveData();
                };
                break;
            case ActionType.StageUnlock:
                confirmMessageText.text = "�������� ��ü�� �ر��մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.UnlockAllStage();
                };
                break;
            case ActionType.ItemReset:
                confirmMessageText.text = "������ �����͸� �ʱ�ȭ�մϴ�";
                confirmAction = () =>
                {
                    SaveLoadManager.ResetItemSaveData();
                };
                break;
            default:
                return;
        }
        confirmWindow.SetActive(true);
    }

    public void ChangeLevelDesignStage()
    {
        if(stageManager.IngameStatus != IngameStatus.LevelDesign)
        {
            stageManager.SetStatus(IngameStatus.LevelDesign);
        }
        else
        {
            stageManager.SetStatus(IngameStatus.Planet);
        }
    }
}
