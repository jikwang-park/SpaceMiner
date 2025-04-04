using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine.AddressableAssets;
using System;
using CsvHelper.Configuration;

public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

     public Dictionary<int, ITableData> TableData { get; protected set; } = new Dictionary<int, ITableData>();

    protected static List<T> LoadCsv<T>(string csv)
    {
        using (var reader = new StringReader(csv))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList();
        }
    }

    protected static string CreateCsv<T>(List<T> data)
    {
        string result = string.Empty;
        using (var memstream = new MemoryStream())
        using (var writer = new StreamWriter(memstream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(data);

            writer.Flush();
            result = Encoding.UTF8.GetString(memstream.ToArray());
        }
        return result;
    }

    public void Load(string fileName)
    {
        var path = string.Format(FormatPath, fileName);

        var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();

        if (textAsset is not null)
        {
            LoadFromText(textAsset.text);
        }
    }

    public abstract void LoadFromText(string text);
    public abstract void Set(List<string[]> data);

    public abstract string GetCsvData();

    public abstract Type DataType { get; }

    protected TData CreateData<TData>(string[] data) where TData : ITableData, new()
    {
        TData datum = new TData();
        datum.Set(data);
        return datum;
    }
}
