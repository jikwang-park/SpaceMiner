using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CustomEditor(typeof(DataTableViewer))]
public class DataTableViewerEditor : Editor
{
    private const string dataTablePathFormat = "{0}/Addressables/DataTables/{1}.csv";

    private bool eventSet = false;

    private void OnEnable()
    {
        if (Application.isPlaying && !eventSet && target is not null)
        {
            eventSet = true;
            Debug.Log("Save Button Function Set");
            var viewer = target as DataTableViewer;
            viewer.SaveButtonText.text = "Save";
            viewer.SaveButton.onClick.RemoveAllListeners();
            viewer.SaveButton.onClick.AddListener(() => SaveTable(viewer.CurrentTableName, viewer.CurrentView));
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Save"))
        {
            var viewer = target as DataTableViewer;
            SaveTable(viewer.CurrentTableName, viewer.CurrentView);
        }
    }

    public void SaveTable(string tableName, DataTableView view)
    {
        string tablePath = string.Format(dataTablePathFormat, Application.dataPath, tableName);
        var table = DataTableManager.GetTable<DataTable>(tableName);
        table.Set(view.GetData());
        File.WriteAllText(tablePath, table.GetCsvData());
        AssetDatabase.Refresh();
    }
}
