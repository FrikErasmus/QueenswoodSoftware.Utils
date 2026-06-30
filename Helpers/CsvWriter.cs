namespace Library.Helpers;

public static class CsvWriter
{
    public static string DataTableToCsv(DataTable dt, string delimiter = ",", bool includeHeaders = true)
    {
        var sb = new StringBuilder();

        if (includeHeaders)
        {
            var headers = dt.Columns.Cast<DataColumn>().Select(c => Escape(c.ColumnName, delimiter));
            sb.AppendLine(string.Join(delimiter, headers));
        }

        foreach (DataRow row in dt.Rows)
        {
            var fields = row.ItemArray.Select(f => Escape(f?.ToString() ?? string.Empty, delimiter));
            sb.AppendLine(string.Join(delimiter, fields));
        }

        return sb.ToString();
    }

    public static string ListToCsv<T>(IEnumerable<T> items, string delimiter = ",", bool includeHeaders = true)
    {
        var list = items.ToList();
        if (list.Count == 0) return string.Empty;

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sb = new StringBuilder();

        if (includeHeaders)
            sb.AppendLine(string.Join(delimiter, properties.Select(p => Escape(p.Name, delimiter))));

        foreach (var item in list)
        {
            var values = properties.Select(p => Escape(p.GetValue(item)?.ToString() ?? string.Empty, delimiter));
            sb.AppendLine(string.Join(delimiter, values));
        }

        return sb.ToString();
    }

    public static byte[] ToBytes(string csv) => Encoding.UTF8.GetBytes(csv);

    private static string Escape(string field, string delimiter)
    {
        if (field.Contains(delimiter) || field.Contains('"') || field.Contains('\n'))
            return $"\"{field.Replace("\"", "\"\"")}\"";
        return field;
    }
}
