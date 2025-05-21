using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTableView : MonoBehaviour
{
    [SerializeField]
    private DataRow dataRowPrefab;

    [SerializeField]
    private TextMeshProUGUI columnCellPrefab;

    public ScrollRect columnScroll;
    public ScrollRect cellScroll;

    [SerializeField]
    private Transform columnContent;
    [SerializeField]
    private Transform cellsContent;

    public int columnCount;

    public string TableName { get; private set; }

    public List<TextMeshProUGUI> columns = new List<TextMeshProUGUI>();
    public List<DataRow> rows = new List<DataRow>();

    public void OnColumnMoved(Vector2 rot)
    {
        cellScroll.horizontalNormalizedPosition = rot.x;
    }

    public void OnCellMoved(Vector2 rot)
    {
        columnScroll.horizontalNormalizedPosition = rot.x;
    }

    public void SetColumns(PropertyInfo[] columnInfos)
    {
        columnCount = columnInfos.Length;
        for (int i = 0; i < columnCount; ++i)
        {
            var text = Instantiate(columnCellPrefab, columnContent);
            columns.Add(text);
            text.text = columnInfos[i].Name;
        }
    }

    public void SetColumns(string[] columns)
    {
        columnCount = columns.Length;
        for (int i = 0; i < columnCount; ++i)
        {
            var text = Instantiate(columnCellPrefab, columnContent);
            text.text = columns[i];
        }
    }

    public void AddRow(string[] rowdata)
    {
        var row = Instantiate(dataRowPrefab, cellsContent);
        row.SetCells(rowdata);
        rows.Add(row);
    }

    public void AddRows(string[][] rowsData)
    {
        for (int i = 0; i < rowsData.GetLength(0); ++i)
        {
            AddRow(rowsData[i]);
        }
    }

    public List<string[]> GetData()
    {
        List<string[]> data = new List<string[]>();

        for (int i = 0; i < rows.Count; ++i)
        {
            var datum = new string[columnCount];

            for (int j = 0; j < columnCount; ++j)
            {
                datum[j] = rows[i].cells[j].CellText;
                //if (properties[j].PropertyType == typeof(int))
                //{
                //    properties[j].SetValue(datum, int.Parse(rows[i].cells[j].CellText));
                //}
                //else if (properties[j].PropertyType == typeof(float))
                //{
                //    properties[j].SetValue(datum, float.Parse(rows[i].cells[j].CellText));
                //}
                //else if (properties[j].PropertyType == typeof(string))
                //{
                //    properties[j].SetValue(datum, rows[i].cells[j].CellText);
                //}
            }
            data.Add(datum);
        }
        return data;
    }

    public void Clear()
    {
        foreach (var column in columns)
        {
            Destroy(column.gameObject);
        }
        columns.Clear();
        foreach (var row in rows)
        {
            Destroy(row.gameObject);
        }
        rows.Clear();
    }

    public void SetTableName(string name)
    {
        TableName = name;
    }

    public void ApplyTable()
    {
        if (string.IsNullOrEmpty(TableName))
        {
            return;
        }
        var table = DataTableManager.GetTable<DataTable>(TableName);
        table.Set(GetData());
    }
}
