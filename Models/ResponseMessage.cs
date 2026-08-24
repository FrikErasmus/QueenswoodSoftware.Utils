namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Standard API response. Payload data, when present, is stored in <see cref="Object"/>.
/// </summary>
public class ResponseMessage
{
    /// <summary>Indicates whether the operation succeeded.</summary>
    public bool Success { get; set; } = true;

    /// <summary>Short user-facing message.</summary>
    public string? ShortDescription { get; set; }

    /// <summary>Detailed error or diagnostic message.</summary>
    public string? FullDescription { get; set; }

    /// <summary>Optional authentication or session token.</summary>
    public string? Token { get; set; }

    /// <summary>Optional payload. Use this property when the operation returns data.</summary>
    public object? Object { get; set; }
}
