namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Parses strings into nullable value types and maps empty values to <see cref="DBNull"/>.
    /// </summary>
    public static class ConvertToNullables
    {
        /// <summary>
        /// Parses a string to a nullable 32-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed <see cref="int"/> or null when parsing fails.</returns>
        public static int? ToNullableInt32(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed <see cref="DateTime"/> or null when parsing fails.</returns>
        public static DateTime? ToNullableDateTime(string s)
        {
            DateTime i;
            if (DateTime.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable byte.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed byte or null when parsing fails.</returns>
        public static byte? ToNullableByte(string s)
        {
            byte i;
            if (byte.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable single-precision float.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed float or null when parsing fails.</returns>
        public static float? ToNullableFloat(string s)
        {
            float i;
            if (float.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable 64-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed <see cref="long"/> or null when parsing fails.</returns>
        public static long? ToNullableLong(string s)
        {
            long i;
            if (long.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable double.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed double or null when parsing fails.</returns>
        public static double? ToNullableDouble(string s)
        {
            double i;
            if (double.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string to a nullable 16-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>Parsed <see cref="short"/> or null when parsing fails.</returns>
        public static short? ToNullableShort(string s)
        {
            short i;
            if (short.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Returns the input value or <see cref="DBNull.Value"/> when null or empty string.
        /// </summary>
        /// <param name="s">The value to test for database null representation.</param>
        /// <returns><paramref name="s"/> when non-empty; otherwise <see cref="DBNull.Value"/>.</returns>
        public static object ToNullableDbNull(object s)
        {
            if (!ReferenceEquals(s, null))
            {
                if (!string.IsNullOrEmpty(s.ToString())) return s;
            }
            return DBNull.Value;
        }
    }
}