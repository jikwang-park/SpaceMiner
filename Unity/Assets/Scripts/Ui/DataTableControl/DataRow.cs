using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRow : MonoBehaviour
{
    [SerializeField]
    private DataCell cellPrefab;

    public List<DataCell> cells;

    public void SetCells(string[] celldata)
    {
        for (int i = 0; i < celldata.Length; ++i)
        {
            var cell = Instantiate(cellPrefab, transform);
            cell.Init(celldata[i]);
            cells.Add(cell);
        }
    }

    public List<string> GetRowData()
    {
        List<string> strings = new List<string>(cells.Count);

        foreach (var cell in cells)
        {
            strings.Add(cell.CellText);
        }
        return strings;
    }
}
