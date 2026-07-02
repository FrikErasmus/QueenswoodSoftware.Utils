namespace QueenswoodSoftware.Utils.Utils
{
    /// <summary>
    /// Provides static helpers for parsing strings into nullable value types and converting values for database use.
    /// </summary>
    public static class NullableUtils
    {
        /// <summary>
        /// Parses a string into a nullable 32-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed integer, or null when parsing fails.</returns>
        public static int? ToNullableInt32(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed date and time, or null when parsing fails.</returns>
        public static DateTime? ToNullableDateTime(string s)
        {
            DateTime i;
            if (DateTime.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable byte.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed byte, or null when parsing fails.</returns>
        public static byte? ToNullableByte(string s)
        {
            byte i;
            if (byte.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable single-precision floating-point number.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed float, or null when parsing fails.</returns>
        public static float? ToNullableFloat(string s)
        {
            float i;
            if (float.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable 64-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed long, or null when parsing fails.</returns>
        public static long? ToNullableLong(string s)
        {
            long i;
            if (long.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable double-precision floating-point number.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed double, or null when parsing fails.</returns>
        public static double? ToNullableDouble(string s)
        {
            double i;
            if (double.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable 16-bit integer.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed short, or null when parsing fails.</returns>
        public static short? ToNullableShort(string s)
        {
            short i;
            if (short.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Parses a string into a nullable boolean.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsed boolean, or null when parsing fails.</returns>
        public static bool? ToNullableBool(string s)
        {
            bool i;
            if (bool.TryParse(s, out i)) return i;
            return null;
        }

        /// <summary>
        /// Returns the input value when it is non-null and non-empty; otherwise returns <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="s">The value to evaluate.</param>
        /// <returns>The original value or <see cref="DBNull.Value"/> for database parameter binding.</returns>
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