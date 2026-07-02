namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// API-related model types for SQL command construction.
/// </summary>
public class Api
{
    /// <summary>
    /// SQL text command with named parameters.
    /// </summary>
    public class SqlStatement
    {
        /// <summary>SQL command text.</summary>
        public string? Command { get; set; }

        /// <summary>Named parameters bound to the command.</summary>
        public List<NameValue> Param { get; set; }
    }
}