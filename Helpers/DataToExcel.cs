namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Exports tabular data to Excel workbooks or CSV streams.
    /// </summary>
    public class DataToExcel
    {
        /// <summary>
        /// Returns XLSX Memory Stream
        /// </summary>
        /// <param name="dt">Data table to write to the worksheet.</param>
        /// <param name="fileName">Unused file name parameter retained for caller compatibility.</param>
        /// <param name="sheetName">Worksheet name in the workbook.</param>
        /// <returns>Memory stream containing the saved XLSX workbook, positioned at the start.</returns>
        public static MemoryStream DataTableToExcel(DataTable dt, string fileName, string sheetName = "Data")
        {
            using var wb = new XLWorkbook();
            wb.Worksheets.Add(dt, sheetName);
            var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Serializes a sequence of objects to a UTF-8 CSV memory stream with a BOM for Excel.
        /// </summary>
        /// <typeparam name="T">Item type whose public properties form CSV columns.</typeparam>
        /// <param name="data">Rows to export.</param>
        /// <returns>Memory stream containing CSV text, positioned at the start.</returns>
        public static MemoryStream ListToCsv<T>(IEnumerable<T> data)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, new UTF8Encoding(true)); // BOM for Excel
            var props = typeof(T).GetProperties();
            // Header
            for (var i = 0; i < props.Length; i++)
            {
                writer.Write(Escape(props[i].Name));
                if (i < props.Length - 1)
                    writer.Write(",");
            }
            writer.WriteLine();
            // Rows
            foreach (var item in data)
            {
                for (var i = 0; i < props.Length; i++)
                {
                    var value = props[i].GetValue(item)?.ToString() ?? string.Empty;
                    writer.Write(Escape(value));
                    if (i < props.Length - 1)
                        writer.Write(",");
                }
                writer.WriteLine();
            }
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Escapes a CSV field value by quoting and doubling embedded quotes when required.
        /// </summary>
        /// <param name="value">Raw field text.</param>
        /// <returns>CSV-safe field value.</returns>
        private static string Escape(string value)
        {
            if (value.Contains("\""))
                value = value.Replace("\"", "\"\"");
            if (value.Contains(",") || value.Contains("\n") || value.Contains("\r"))
                value = $"\"{value}\"";
            return value;
        }
    }
}