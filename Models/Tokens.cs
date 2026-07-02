namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Session token metadata for authenticated users.
/// </summary>
public class Tokens
{
    /// <summary>Unique session identifier.</summary>
    public string? SessionId { get; set; }

    /// <summary>Associated user identifier.</summary>
    public string? UserId { get; set; }

    /// <summary>When the session was created.</summary>
    public DateTime LoginDate { get; set; }

    /// <summary>Last recorded activity timestamp.</summary>
    public DateTime LastActivityTime { get; set; }

    /// <summary>Whether the session is currently active.</summary>
    public bool IsActive { get; set; }

    /// <summary>Whether the session record is marked deleted.</summary>
    public bool Deleted { get; set; }
}