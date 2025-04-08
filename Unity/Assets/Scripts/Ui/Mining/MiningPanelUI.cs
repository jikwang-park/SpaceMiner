using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MiningRobotInventoryManager;

public class MiningPanelUI : MonoBehaviour
{
    [SerializeField]
    private MiningRobotMergePopupUI robotMergePopupUI;

    private void Awake()
    {
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
}
