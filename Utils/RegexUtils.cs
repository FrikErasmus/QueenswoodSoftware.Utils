namespace QueenswoodSoftware.Utils.Utils
{
    /// <summary>
    /// Provides static helpers for common regular expression operations.
    /// </summary>
    public static class RegexUtils
    {
        /// <summary>
        /// Checks if a string matches a given regex pattern.
        /// </summary>
        /// <param name="input">The input string to check.</param>
        /// <param name="pattern">The regex pattern to match.</param>
        /// <returns>True if the input string matches the pattern, otherwise false.</returns>
        public static bool IsMatch(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(input, pattern);
            }
            catch (RegexParseException ex)
            {
                Console.WriteLine($"Regex parse error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Replaces all occurrences of a regex pattern in a string with a replacement string.
        /// </summary>
        /// <param name="input">The input string to modify.</param>
        /// <param name="pattern">The regex pattern to match.</param>
        /// <param name="replacement">The replacement string.</param>
        /// <returns>The modified string.</returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
            {
                return input;
            }
            try
            {
                return Regex.Replace(input, pattern, replacement);
            }
            catch (RegexParseException ex)
            {
                Console.WriteLine($"Regex parse error: {ex.Message}");
                return input;
            }
        }

        /// <summary>
        /// Splits a string into an array of substrings based on a regex pattern.
        /// </summary>
        /// <param name="input">The input string to split.</param>
        /// <param name="pattern">The regex pattern to split by.</param>
        /// <returns>An array of substrings.</returns>
        public static string[] Split(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
            {
                return new string[0];
            }
            try
            {
                return Regex.Split(input, pattern);
            }
            catch (RegexParseException ex)
            {
                Console.WriteLine($"Regex parse error: {ex.Message}");
                return new string[0];
            }
        }

        /// <summary>
        /// Escapes special characters in a string for use in a regex pattern.
        /// </summary>
        /// <param name="input">The input string to escape.</param>
        /// <returns>The escaped string.</returns>
        public static string Escape(string input)
        {
            if (input == null)
            {
                return null;
            }
            return Regex.Escape(input);
        }
    }
}