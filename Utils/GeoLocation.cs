namespace Library.Utils
{
    /// <summary>
    /// Provides static helpers for interpreting GPS location data such as heading, speed, and freshness.
    /// </summary>
    public class GeoLocation
    {
        /// <summary>
        /// Converts a compass course in degrees to a 16-point compass direction name.
        /// </summary>
        /// <param name="course">The course angle in degrees.</param>
        /// <returns>The compass direction label (for example, "North" or "Northeast").</returns>
        public static string GetCompassDirection(double course)
        {
            // Compass directions split into 16 sectors of 22.5 degrees each
            string[] directions =
            [
                "North", "North-Northeast", "Northeast", "East-Northeast",
                "East", "East-Southeast", "Southeast", "South-Southeast",
                "South", "South-Southwest", "Southwest", "West-Southwest",
                "West", "West-Northwest", "Northwest", "North-Northwest"
            ];

            // Normalize angle
            course = (course % 360 + 360) % 360;

            // Convert course into index (0-15)
            var index = (int)Math.Round(course / 22.5) % 16;
            return directions[index];
        }

        /// <summary>
        /// Converts location speed from meters per second to kilometers per hour.
        /// </summary>
        /// <param name="location">The location containing speed in meters per second.</param>
        /// <returns>The speed in km/h, or null when speed is unavailable.</returns>
        public static double? GetSpeedKmh(Location location)
        {
            return location.Speed * 3.6;
        }

        /// <summary>
        /// Determines whether a location is moving at or above a minimum speed threshold.
        /// </summary>
        /// <param name="location">The location to evaluate.</param>
        /// <param name="minSpeedKmh">The minimum speed in km/h considered movement. Defaults to 1.0.</param>
        /// <returns>True if the location speed meets or exceeds the threshold; otherwise false.</returns>
        public static bool IsMoving(Location location, double minSpeedKmh = 1.0)
        {
            var speed = GetSpeedKmh(location);
            return speed >= minSpeedKmh;
        }

        /// <summary>
        /// Determines whether a location timestamp is within the specified maximum age.
        /// </summary>
        /// <param name="location">The location to evaluate.</param>
        /// <param name="maxAge">The maximum allowed age of the location reading.</param>
        /// <returns>True if the location is recent enough; otherwise false.</returns>
        public static bool IsLocationRecent(Location location, TimeSpan maxAge)
        {
            return DateTimeOffset.UtcNow - location.Timestamp <= maxAge;
        }
    }
}
