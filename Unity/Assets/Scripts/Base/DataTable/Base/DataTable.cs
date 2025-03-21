using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

public abstract class DataTable
{
    public static readonly string FormatPath = "DataTables/{0}";

    public static List<T> LoadCsv<T>(string csv)
    {
        using (var reader = new StringReader(csv))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList();
        }
    }

    public abstract void Load(string path);
}
