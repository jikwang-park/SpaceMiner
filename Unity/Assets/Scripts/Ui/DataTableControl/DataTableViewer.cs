using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataTableViewer : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown tableDropdown;
    [SerializeField]
    private DataTableView dataTableViewPrefab;
    [SerializeField]
    private Transform tableView;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Toggle inputToggle;
    [field: SerializeField]
    public Button SaveButton { get; private set; }
    [field: SerializeField]
    public TextMeshProUGUI SaveButtonText { get; private set; }

    private Dictionary<string, DataTableView> views = new Dictionary<string, DataTableView>();

    public DataTableView CurrentView { get; private set; }
    private int currentIndex;

    public string CurrentTableName
    {
        get
        {
            return tableDropdown.options[currentIndex].text;
        }
    }

    private void Awake()
    {
        ResetTables();
    }

    private void SetTable(KeyValuePair<string, DataTable> table)
    {
        var dict = table.Value.TableData;
        var dataType = table.Value.DataType;

        var properties = dataType.GetProperties();

        AddDropDownOption(table.Key);
        var tableView = Instantiate(dataTableViewPrefab, this.tableView);
        tableView.SetTableName(table.Key);
        views.Add(table.Key, tableView);
        tableView.SetColumns(properties);

        foreach (var data in dict)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            tableView.AddRow(values);
        }
        tableView.gameObject.SetActive(false);
    }

    private void AddDropDownOption(string name)
    {
        TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
        optionData.text = name;
        tableDropdown.options.Add(optionData);
    }

    public void OnDropDownChanged(int index)
    {
        if (CurrentView != null)
        {
            CurrentView.gameObject.SetActive(false);
        }
        currentIndex = index;
        CurrentView = views[tableDropdown.options[index].text];
        CurrentView.gameObject.SetActive(true);
    }

    public void ResetViewer()
    {
        tableDropdown.ClearOptions();

        foreach (var view in views)
        {
            Destroy(view.Value.gameObject);
        }

        ResetTables();
    }

    public void AddEmptyRow()
    {
        var table = views.ElementAt(currentIndex);
        table.Value.AddRow(new string[table.Value.columnCount]);
    }

    private void ResetTables()
    {
        tableDropdown.ClearOptions();
        views.Clear();

        foreach (var table in DataTableManager.Tables)
        {
            SetTable(table);
        }

        tableDropdown.value = 0;
        tableDropdown.RefreshShownValue();
        OnDropDownChanged(0);
    }

    public void ResetTable()
    {
        var table = DataTableManager.GetTable<DataTable>(CurrentView.TableName);
        var dict = table.TableData;
        var dataType = table.DataType;
        var properties = dataType.GetProperties();

        CurrentView.Clear();
        CurrentView.SetColumns(properties);

        foreach (var data in dict)
        {
            string[] values = new string[properties.Length];

            for (int i = 0; i < properties.Length; ++i)
            {
                values[i] = properties[i].GetValue(data.Value).ToString();
            }

            CurrentView.AddRow(values);
        }
    }

    public void ApplyCurrentViewTable()
    {
        CurrentView.ApplyTable();
    }

    public void OnInsert(bool isOn)
    {
        if (isOn)
        {
            inputToggle.image.color = Color.grey;
        }
        else
        {
            try
            {
                string text = inputField.text;

                int previous = currentIndex;

                if (!string.IsNullOrEmpty(text))
                {
                    string csvText = text.Replace('\t', ',');

                    var currentTable = DataTableManager.GetTable<DataTable>(tableDropdown.options[currentIndex].text);
                    currentTable.LoadFromText(csvText);
                    ResetTables();
                }

                tableDropdown.value = previous;
            }
            catch (Exception e)
            {
                Debug.LogError($"Table Set Failed \n{e}");
            }
            inputField.text = string.Empty;
            inputToggle.image.color = Color.white;
        }
        inputField.gameObject.SetActive(isOn);
    }
}
