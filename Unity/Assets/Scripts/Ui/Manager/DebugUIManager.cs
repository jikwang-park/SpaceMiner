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

    private HashSet<int> itemids = new HashSet<int>();

    private Coroutine coRowSet;

    private void Start()
    {
        coRowSet = StartCoroutine(CoRowActive());
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
}
