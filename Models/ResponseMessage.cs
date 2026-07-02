namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Standard API response for operations that return no payload.
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

    /// <summary>Payload object or serialized result data.</summary>
    public object? Object { get; set; }
}

/// <summary>
/// Standard API response with a strongly-typed data payload.
/// </summary>
/// <typeparam name="T">The type of the returned data.</typeparam>
public class ResponseMessage<T> : ResponseMessage
{
    /// <summary>The typed result payload.</summary>
    public T? Data { get; set; }
}