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

    private DataTable table;

    public void OnColumnMoved(Vector2 rot)
    {
        cellScroll.horizontalNormalizedPosition = rot.x;
    }

    public void OnCellMoved(Vector2 rot)
    {
        columnScroll.horizontalNormalizedPosition = rot.x;
    }

    public void Set(string name, DataTable table)
    {
        TableName = name;
        this.table = table;
    }

    private void Start()
    {
        var dict = table.TableData;
        var dataType = table.DataType;

        var properties = dataType.GetProperties();

        SetColumns(properties);

        foreach (var data in dict)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            AddRow(values);
        }
    }

    public void ResetTable()
    {
        var dict = table.TableData;
        var dataType = table.DataType;
        var properties = dataType.GetProperties();
        Clear();

        foreach (var data in dict)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            AddRow(values);
        }
    }

    private void SetColumns(PropertyInfo[] columnInfos)
    {
        columnCount = columnInfos.Length;
        for (int i = 0; i < columnCount; ++i)
        {
            var text = Instantiate(columnCellPrefab, columnContent);
            columns.Add(text);
            text.text = columnInfos[i].Name;
        }
    }

    private void SetColumns(string[] columns)
    {
        columnCount = columns.Length;
        for (int i = 0; i < columnCount; ++i)
        {
            var text = Instantiate(columnCellPrefab, columnContent);
            text.text = columns[i];
        }
    }

    private void AddRow(string[] rowdata)
    {
        var row = Instantiate(dataRowPrefab, cellsContent);
        row.SetCells(rowdata);
        rows.Add(row);
    }

    public void AddEmptyRow()
    {
        AddRow(new string[columnCount]);
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

    public void ApplyTable()
    {
        if (string.IsNullOrEmpty(TableName))
        {
            return;
        }
        table.Set(GetData());
    }
}
