using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MiningRobotInventoryManager;

public class MiningPanelUI : MonoBehaviour
{
    [SerializeField]
    private MiningRobotMergePopupUI robotMergePopupUI;
    [SerializeField]
    private SetMiningRobotToPlanetUI setRobotToPlanetUI;
    private List<int> planetIds = new List<int>();
    private StageManager stageManager;
    private void Awake()
    {
        stageManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<StageManager>();
        planetIds = DataTableManager.PlanetTable.GetIds();
        onRequestMerge += ShowMergePopup;
    }
    private void ShowMergePopup(int mergedRobotId, MergeResponseCallback callback)
    {
        RobotMergeTable.Data mergeData = DataTableManager.RobotMergeTable.GetData(mergedRobotId);
        if (mergeData == null)
        {
            callback.Invoke(false);
            return;
        }
        robotMergePopupUI.Initialize(mergedRobotId, callback);
        robotMergePopupUI.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        OnClickPlanetButton(0);
    }
    public void OnClickPlanetButton(int index)
    {
        setRobotToPlanetUI.Initialize(planetIds[index]);
        currentPlanetId = planetIds[index];
    }
    public void OnClickSortButton()
    {
        MiningRobotInventoryManager.SortInventorySlots();
    }

    public void OnClickMoveButton()
    {
        Variables.planetMiningID = currentPlanetId;
        stageManager.SetStatus(IngameStatus.Mine);
    }
}
