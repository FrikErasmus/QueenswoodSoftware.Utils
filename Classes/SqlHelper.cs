using Microsoft.Data.SqlClient;
namespace Library.Classes;
/// <summary>
/// Defines SQL execution operations for transactions, selects, and stored-procedure calls.
/// </summary>
public interface ISqlHelper
{
    /// <summary>
    /// Executes a list of SQL commands within a single transaction.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> indicating success or failure.</returns>
    public ResponseMessage SqlTransaction(List<SqlCommand> commands, SqlConnection sqlConnection);
    /// <summary>
    /// Executes a SQL select command and returns the result set.
    /// </summary>
    /// <param name="command">The select command to execute.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing serialized results or an error.</returns>
    public ResponseMessage SqlSelect(SqlCommand command, SqlConnection sqlConnection);
    /// <summary>
    /// Executes ad hoc SQL text and maps the result set to model instances.
    /// </summary>
    /// <param name="command">The SQL text to execute.</param>
    /// <param name="model">A template object used to determine the target model type.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing populated model objects or an error.</returns>
    public ResponseMessage GenericSelect(string command, object model, SqlConnection sqlConnection);
    /// <summary>
    /// Builds and executes a stored procedure from the properties of a model object.
    /// </summary>
    /// <param name="storedProcedure">The stored procedure name.</param>
    /// <param name="modelObject">The object whose properties become command parameters.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing the procedure result or an error.</returns>
    public ResponseMessage SqlBuilder(string storedProcedure, object modelObject, SqlConnection sqlConnection);
}
/// <summary>
/// Executes SQL commands with retry, transaction handling, and model mapping support.
/// </summary>
public class SqlHelper : ISqlHelper
{
    /// <summary>
    /// Executes a list of SQL commands within a single transaction.
    /// </summary>
    /// <param name="commands">The commands to execute.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> indicating success or failure.</returns>
    public ResponseMessage SqlTransaction(List<SqlCommand> commands, SqlConnection sqlConnection)
    {
        var restart = 0;
        try
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }
        Restart:
            var transaction = sqlConnection.BeginTransaction();
            try
            {
                foreach (var command in commands)
                {
                    command.Connection = sqlConnection;
                    command.Transaction = transaction;
                    if (command.CommandType == CommandType.StoredProcedure)
                    {
                        command.ExecuteScalar();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();
            }
            catch (TimeoutException ex)
            {
                if (restart < 5)
                {
                    Log.Error(ex, "Timeout occurred in SQL Transaction");
                    transaction.Rollback();
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                    restart++;
                    goto Restart;
                }
                transaction.Rollback();
                Log.Error(ex, "Timeout occurred in SQL Transaction. Rollback occurred");
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                return new ResponseMessage
                {
                    Success = false,
                    ShortDescription = "Timeout occurred in SQL Transaction. Rollback occurred",
                    FullDescription = ex.Message
                };
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                Log.Error(ex, "Error occurred in SQL Transaction");
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                return new ResponseMessage
                {
                    Success = false,
                    ShortDescription = "Unable to save changes.",
                    FullDescription = ex.Message
                };
            }
            catch (DataException ex)
            {
                transaction.Rollback();
                Log.Error(ex, "Error occurred in SQL Transaction");
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                return new ResponseMessage
                {
                    Success = false,
                    ShortDescription = "Unable to save changes.",
                    FullDescription = ex.Message
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(ex, "Error occurred in SQL Transaction");
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                return new ResponseMessage
                {
                    Success = false,
                    ShortDescription = "Unable to save changes.",
                    FullDescription = ex.Message
                };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred in SQL Transaction");
            return new ResponseMessage
            {
                Success = false,
                ShortDescription = "Unable to save changes.",
                FullDescription = ex.Message
            };
        }
        finally
        {
            if (sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
        }
        return new ResponseMessage { Success = true };
    }
    /// <summary>
    /// Receives a SQL select command and returns a data table with the results.
    /// </summary>
    /// <param name="command">The select command to execute.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing serialized results or an error.</returns>
    public ResponseMessage SqlSelect(SqlCommand command, SqlConnection sqlConnection)
    {
        var restart = 0;
        var dataTable = new DataTable();
    Restart:
        try
        {
            if (sqlConnection.State != ConnectionState.Open) sqlConnection.Open();
            try
            {
                command.Connection = sqlConnection;
                var reader = new SqlDataAdapter(command);
                reader.Fill(dataTable);
            }
            catch (System.TimeoutException timeOutException)
            {
                if (restart < 5)
                {
                    Log.Error($"Timeout occurred in SQL Select: Retry: {restart}", timeOutException);
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                    restart++;
                    goto Restart;
                }
                Log.Error(timeOutException, "Timeout occurred in SQL Select.");
                return new ResponseMessage { Success = false, ShortDescription = timeOutException.Message, FullDescription = timeOutException.Message };
            }
            catch (SqlException sqlError)
            {
                Log.Error(sqlError, "Error occurred in SQL Select");
                Log.Information(command.CommandText.ToString());
                return new ResponseMessage { Success = false, ShortDescription = "Error occurred during SQL Transaction", FullDescription = sqlError.Message };
            }
        }
        catch (SqlException ex)
        {
            Log.Error("Unable to connect to the database.", ex);
            return new ResponseMessage { Success = false, ShortDescription = "Unable to connect to the database.", FullDescription = ex.Message };
        }
        finally
        {
            if (sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
        }
        if (dataTable.Rows.Count == 0)
        {
            return new ResponseMessage
            {
                Object = null,
                ShortDescription = "0 Record Found",
                Success = true
            };
        }
        return new ResponseMessage { Object = JsonConvert.SerializeObject(dataTable), Success = true };
    }
    /// <summary>
    /// Executes ad hoc SQL text and maps the result set to model instances.
    /// </summary>
    /// <param name="command">The SQL text to execute.</param>
    /// <param name="model">A template object used to determine the target model type.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing populated model objects or an error.</returns>
    public ResponseMessage GenericSelect(string command, object model, SqlConnection sqlConnection)
    {
        var sqlCommand = new SqlCommand(command)
        {
            CommandType = CommandType.Text
        };
        var result = SqlSelect(sqlCommand, sqlConnection);
        if (!result.Success)
            return result;
        if (result.Object == null)
            return new ResponseMessage { Object = new List<object>(), Success = true, ShortDescription = result.ShortDescription };
        DataTable dataTable;
        if (result.Object is DataTable dt)
        {
            dataTable = dt;
        }
        else if (result.Object is string json)
        {
            dataTable = JsonConvert.DeserializeObject<DataTable>(json) ?? new DataTable();
        }
        else
        {
            return new ResponseMessage
            {
                Success = false,
                ShortDescription = "Unexpected SQL result payload.",
                FullDescription = $"Result type was {result.Object.GetType().FullName}"
            };
        }
        return new ResponseMessage { Object = ModelInjector.PopulateModel(model, dataTable), Success = true };
    }
    /// <summary>
    /// Creates a CRUD based on the model properties.
    /// </summary>
    /// <param name="storedProcedure">The stored procedure name.</param>
    /// <param name="modelObject">The object whose properties become command parameters.</param>
    /// <param name="sqlConnection">The SQL connection to use.</param>
    /// <returns>A <see cref="ResponseMessage"/> containing the procedure result or an error.</returns>
    public ResponseMessage SqlBuilder(string storedProcedure, object modelObject, SqlConnection sqlConnection)
    {
        var sqlCommand = new SqlCommand(storedProcedure)
        {
            CommandType = CommandType.StoredProcedure
        };
        foreach (var prop in modelObject.GetType().GetProperties())
        {
            var propertyValue = prop.GetValue(modelObject, null) == null ? DBNull.Value : prop.GetValue(modelObject, null);
            sqlCommand.Parameters.AddWithValue($"@{prop.Name}", propertyValue);
        }
        return SqlSelect(sqlCommand, sqlConnection);
    }
    /// <summary>
    /// Builds a stored-procedure command from the properties of a model object.
    /// </summary>
    /// <param name="storedProcedure">The stored procedure name.</param>
    /// <param name="modelObject">The object whose properties become command parameters.</param>
    /// <returns>A configured <see cref="SqlCommand"/> ready for execution.</returns>
    public SqlCommand BuildSPFromObject(string storedProcedure, object modelObject)
    {
        var sqlCommand = new SqlCommand(storedProcedure)
        {
            CommandType = CommandType.StoredProcedure
        };
        foreach (var prop in modelObject.GetType().GetProperties())
        {
            var propertyValue = prop.GetValue(modelObject, null) == null ? DBNull.Value : prop.GetValue(modelObject, null);
            sqlCommand.Parameters.AddWithValue($"@{prop.Name}", propertyValue);
        }
        return sqlCommand;
    }
    /// <summary>
    /// Builds a stored-procedure command without parameters.
    /// </summary>
    /// <param name="storedProcedure">The stored procedure name.</param>
    /// <returns>A configured <see cref="SqlCommand"/> ready for execution.</returns>
    public SqlCommand BuildSPNoObject(string storedProcedure)
    {
        return new SqlCommand(storedProcedure)
        {
            CommandType = CommandType.StoredProcedure
        };
    }
}
/// <summary>
/// Maps <see cref="DataRow"/> and <see cref="DataTable"/> results onto model objects.
/// </summary>
public static class ModelInjector
{
    /// <summary>
    /// Populates a new instance of the target type from a single data row.
    /// </summary>
    /// <param name="target">A template object used to determine the target type.</param>
    /// <param name="source">The data row containing column values.</param>
    /// <returns>A populated model instance.</returns>
    public static object PopulateModel(object target, DataRow source)
    {
        var instance = Activator.CreateInstance(target.GetType()); // Create a variable with the dataType of the object passed
        // for each column in the Row passed
        foreach (DataColumn column in source.Table.Columns)
        {
            var activeTarget = target.GetType().GetProperty(column.ColumnName); // Create a variable with the dataType of the current column
            if (activeTarget == null) continue;
            var value = source[column];
            if (value == DBNull.Value) continue;
            activeTarget.SetValue(instance, value); // assign the value to the column in the object
        }
        return instance!;
    }
    /// <summary>
    /// Populates a list of model instances from all rows in a data table.
    /// </summary>
    /// <param name="target">A template object used to determine the target type.</param>
    /// <param name="source">The data table containing result rows.</param>
    /// <returns>A list of populated model instances.</returns>
    public static List<object> PopulateModel(object target, DataTable source)
    {
        var result = new List<object>();
        try
        {
            foreach (DataRow row in source.Rows)
            {
                result.Add(PopulateModel(target, row));
            }
        }
        catch
        {
            //Redundant
        }
        return result;
    }
}
