using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField]
    private DebugItemRow itemRowPrefab;

    [SerializeField]
    private Transform itemViewContent;

    private List<DebugItemRow> itemRows = new List<DebugItemRow>();

    private void Start()
    {
        var ids = DataTableManager.ItemTable.GetIds();

        foreach (var id in ids)
        {
            var row = Instantiate(itemRowPrefab,itemViewContent);
            row.Set(id);
            itemRows.Add(row);
        }
    }

    public void Refresh()
    {
        foreach (var row in itemRows)
        {
            row.Refresh();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(gameObject.activeSelf);
    }
}
