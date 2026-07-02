namespace QueenswoodSoftware.Utils.Models;

/// <summary>
/// Geographic location with optional GPS metadata and derived flags.
/// </summary>
public class Location
{
    /// <summary>Latitude in decimal degrees.</summary>
    public double Latitude { get; set; }

    /// <summary>Longitude in decimal degrees.</summary>
    public double Longitude { get; set; }

    /// <summary>Horizontal accuracy in meters, when available.</summary>
    public double? Accuracy { get; set; }

    /// <summary>Altitude in meters, when available.</summary>
    public double? Altitude { get; set; }

    /// <summary>Speed in meters per second, when available.</summary>
    public double? Speed { get; set; }

    /// <summary>Course in degrees, when available.</summary>
    public double? Course { get; set; }

    /// <summary>When the location reading was captured.</summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>Human-readable compass direction (for example, Southwest).</summary>
    public string? CompassDirection { get; set; }

    /// <summary>Resolved street address, when geocoded.</summary>
    public string? Address { get; set; }

    /// <summary>True when speed indicates movement (approximately 1 km/h or faster).</summary>
    public bool IsMoving => Speed.HasValue && Speed.Value * 3.6 >= 1.0;

    /// <summary>True when altitude is present.</summary>
    public bool HasAltitude => Altitude.HasValue;

    /// <summary>True when course is present.</summary>
    public bool HasCourse => Course.HasValue;

    /// <summary>True when speed is present.</summary>
    public bool HasSpeed => Speed.HasValue;
}