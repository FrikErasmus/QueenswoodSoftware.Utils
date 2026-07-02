namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Simple name/value pair for parameters and configuration entries.
/// </summary>
public class NameValue
{
    /// <summary>Parameter or field name.</summary>
    public string? Name { get; set; }

    /// <summary>Parameter or field value.</summary>
    public string? Value { get; set; }

    /// <summary>Parameter or object value.</summary>
    public object? Obj { get; set; }
}