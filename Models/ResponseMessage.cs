namespace Library.Models;

/// <summary>
/// Standard API response wrapper used across Library callers.
/// </summary>
public class ResponseMessage
{
    /// <summary>Indicates whether the operation succeeded.</summary>
    public bool Success { get; set; } = true;

    /// <summary>Detailed error or diagnostic message.</summary>
    public string? FullDescription { get; set; }

    /// <summary>Short user-facing message.</summary>
    public string? ShortDescription { get; set; }

    /// <summary>Optional authentication or session token.</summary>
    public string? Token { get; set; }

    /// <summary>Payload object or serialized result data.</summary>
    public object? Object { get; set; }
}
