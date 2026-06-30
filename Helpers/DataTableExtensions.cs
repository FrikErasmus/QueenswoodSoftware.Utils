namespace Library.Helpers
{
    public class DataTableExtensions
    {
        /// <summary>
        /// Converts the rows of the specified DataTable into a list of objects of type T, mapping column values to
        /// matching property names.
        /// </summary>
        /// <remarks>Only properties of T that have matching column names in the DataTable are set.
        /// Properties are set using Convert.ChangeType; type mismatches may result in exceptions at runtime. Columns
        /// with DBNull values are ignored and the corresponding properties are left at their default values.</remarks>
        /// <typeparam name="T">The type of objects to create from the DataTable rows. Must have a parameterless constructor.</typeparam>
        /// <param name="dataTable">The DataTable whose rows will be converted to objects. Each column is mapped to a property of T with the
        /// same name.</param>
        /// <returns>A list of objects of type T, each representing a row from the DataTable. The list will be empty if the
        /// DataTable contains no rows.</returns>
        public List<T> ToList<T>(DataTable dataTable) where T : new()
        {
            var properties = typeof(T).GetProperties();
            return dataTable.Rows.Cast<DataRow>().Select(row =>
            {
                var obj = new T();
                foreach (var prop in properties)
                {
                    if (!dataTable.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value) continue;
                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        var value = row[prop.Name];

                        if (targetType == typeof(string))
                            prop.SetValue(obj, value.ToString());
                        else if (targetType == typeof(long))
                            prop.SetValue(obj, Convert.ToInt64(value));
                        else if (targetType == typeof(int))
                            prop.SetValue(obj, Convert.ToInt32(value));
                        else if (targetType == typeof(bool))
                            prop.SetValue(obj, Convert.ToBoolean(value));
                        else if (targetType == typeof(decimal))
                            prop.SetValue(obj, Convert.ToDecimal(value));
                        else if (targetType == typeof(DateTime))
                            prop.SetValue(obj, Convert.ToDateTime(value));
                        else
                            prop.SetValue(obj, Convert.ChangeType(value, targetType));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error mapping {prop.Name}: {ex.Message}");
                    }
                }
                return obj;
            }).ToList();
        }
    }
}