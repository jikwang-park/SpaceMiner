using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MiningRobotInventoryManager;

public class MiningRobotMergePopupUI : MonoBehaviour
{
    [SerializeField]
    private MiningRobotIcon beforeIcon;
    [SerializeField] 
    private MiningRobotIcon afterIcon;
    [SerializeField]
    private TextMeshProUGUI percentText;

    private MergeResponseCallback responseCallback;

    public void Initialize(int robotId, MergeResponseCallback callback)
    {
        var data = DataTableManager.RobotMergeTable.GetData(robotId);

        var beforeData = DataTableManager.RobotTable.GetData(data.NeedRobotID);
        var afterData = DataTableManager.RobotTable.GetData(data.ResultRobotID);

        responseCallback = callback;
        beforeIcon.Initialize(beforeData.Grade);
        afterIcon.Initialize(afterData.Grade);
        percentText.text = $"{data.Probability} %";
    }

    public void OnClickConfirmButton()
    {
        responseCallback?.Invoke(true);
        gameObject.SetActive(false);
    }
    public void OnClickCancelButton()
    {
        responseCallback?.Invoke(false);
        gameObject.SetActive(false);
    }
}
