using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DungeonSelectTab : MonoBehaviour
{
    [SerializeField]
    private Transform scrollContent;

    [SerializeField]
    private string dungeonRowPrefab = "Prefabs/UI/DungeonEnterRow";

    [SerializeField]
    private Dungeon1EnterWindow dungeon1EnterWindow;

    [SerializeField]
    private Dungeon2EnterWindow dungeon2EnterWindow;

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
        switch (type)
        {
            case 1:
                dungeon1EnterWindow.gameObject.SetActive(true);
                dungeon1EnterWindow.ShowPopup();
                break;
            case 2:
                dungeon2EnterWindow.gameObject.SetActive(true);
                dungeon2EnterWindow.ShowPopup();
                break;
        }
    }
}
