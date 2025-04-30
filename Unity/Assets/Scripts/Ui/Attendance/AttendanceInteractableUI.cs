using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AttendanceInteractableUI : MonoBehaviour
{
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private LocalizationText describeText;

    private const string prefabFormat = "Prefabs/UI/AttendanceElement";

    public void Initialize(AttendanceTable.Data data)
    {
        describeText.SetString(data.DetailStringID);

        int attendanceCount = data.Period;
        int existingCount = contentParent.childCount;

        for (int i = 0; i < existingCount; i++)
        {
            var go = contentParent.GetChild(i).gameObject;
            bool shouldShow = i < attendanceCount;
            go.SetActive(shouldShow);
            if (shouldShow)
            {
                var elem = go.GetComponent<AttendanceElements>();
                elem.Initialize(
                    DataTableManager.AttendanceRewardTable.GetData(data.ID, i + 1)
                );
            }
        }

        int needToCreate = Mathf.Max(0, attendanceCount - existingCount);
        for (int j = 0; j < needToCreate; j++)
        {
            int dayIndex = existingCount + j + 1;

            Addressables.InstantiateAsync(prefabFormat, contentParent)
                      .Completed += handle =>
                      {
                          if (handle.Status != AsyncOperationStatus.Succeeded)
                              return;

                          var newGo = handle.Result;
                          newGo.SetActive(true);
                          var elem = newGo.GetComponent<AttendanceElements>();
                          elem.Initialize(
                              DataTableManager.AttendanceRewardTable.GetData(data.ID, dayIndex)
                          );
                      };
        }

    }
}
