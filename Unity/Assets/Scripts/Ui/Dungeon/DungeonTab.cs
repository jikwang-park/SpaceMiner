using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DungeonPage : MonoBehaviour
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private const string dungeonRowPrefab = "Prefabs/UI/DungeonEnterRow";

    [SerializeField]
    private DungeonPopup dungeonPopup;

    private void Start()
    {
        List<int> dungeons = DataTableManager.DungeonTable.DungeonTypes;

        foreach (int dungeonType in dungeons)
        {
            var handle = Addressables.InstantiateAsync(dungeonRowPrefab, scrollContent);
            handle.Completed += (handle) => OnRowLoaded(handle, dungeonType);
        }
    }

    private void OnRowLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> handle, int type)
    {
        if (!handle.IsDone)
        {
            return;
        }
        if (handle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            return;
        }
        var row = handle.Result.GetComponent<DungeonEnterRow>();
        row.SetType(type);
        row.OnButtonClicked += ShowWindow;
    }

    public void ShowWindow(int type)
    {
        Variables.currentDungeonType = type;
        dungeonPopup.gameObject.SetActive(true);
        dungeonPopup.ShowPopup();
    }
}
