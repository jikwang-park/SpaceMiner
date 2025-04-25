using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SelectAttendancePanelUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;

    private const string prefabFormat = "Prefabs/UI/SelectAttendanceButton";

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        var attendances = DataTableManager.AttendanceTable.GetList();

        foreach(Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var totalCount = attendances.Count;
        var completedCount = 0;

        foreach(var attendance in attendances)
        {
            Addressables.InstantiateAsync(prefabFormat, contentParent).Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject elementObj = handle.Result;
                    SelectAttendanceButton selectGachaButton = elementObj.GetComponent<SelectAttendanceButton>();
                    selectGachaButton.Initialize(attendance);
                    selectGachaButton.parent = this;
                }

                completedCount++;
                if (totalCount == completedCount)
                {
                    OnClickSelectAttendanceButton(attendances.First().ID);
                }
            };
        }
    }
    public void OnClickSelectAttendanceButton(int attendanceId)
    {

    }
}
