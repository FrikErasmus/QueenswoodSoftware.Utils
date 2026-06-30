using System.Numerics;
using System.Text.Json.Nodes;

namespace Library.Helpers
{
    /// <summary>
    /// Converts <see cref="DataTable"/> rows to JSON strings or strongly typed objects.
    /// </summary>
    public class ConvertDataTableToJson
    {
        /// <summary>
        /// Serializes all rows of a <see cref="DataTable"/> to a JSON array of column-name dictionaries.
        /// </summary>
        /// <param name="dataTable">The table whose rows are serialized.</param>
        /// <returns>JSON string representing the table data.</returns>
        public string Convert(DataTable dataTable)
        {
            var data = dataTable.Rows.OfType<DataRow>()
                .Select(row => dataTable.Columns.OfType<DataColumn>()
                    .ToDictionary(col => col.ColumnName, c => row[c]));

            return System.Text.Json.JsonSerializer.Serialize(data);
        }

        /// <summary>
        /// Serializes a <see cref="DataTable"/> to JSON and deserializes it to type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type for deserialization.</typeparam>
        /// <param name="dataTable">The table whose rows are serialized.</param>
        /// <returns>Deserialized instance of <typeparamref name="T"/>.</returns>
        public T Convert<T>(DataTable dataTable)
        {
            var data = dataTable.Rows.OfType<DataRow>()
                .Select(row => dataTable.Columns.OfType<DataColumn>()
                    .ToDictionary(col => col.ColumnName, c => row[c]));

            return JsonConvert.DeserializeObject<T>(System.Text.Json.JsonSerializer.Serialize(data))!;
        }
    }
}
