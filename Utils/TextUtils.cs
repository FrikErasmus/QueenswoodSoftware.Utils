namespace QueenswoodSoftware.Utils.Utils
{
    /// <summary>
    /// Provides static helpers for inspecting, transforming, and encoding text values.
    /// </summary>
    public class TextUtils
    {
        /// <summary>
        /// Uppercases the first character of a string using a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The string with its first character uppercased, or the original value when null or empty.</returns>
        public static string UppercaseFirstCharacterStringBuilder(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var sb = new StringBuilder(input);
            if (char.IsLower(sb[0]))
            {
                sb[0] = char.ToUpper(sb[0]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether a string can be parsed as a numeric value.
        /// </summary>
        /// <param name="input">The string to test.</param>
        /// <returns>True when the string represents a number; otherwise false.</returns>
        public static bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }

        /// <summary>
        /// Compares two strings after removing non-alphanumeric characters and sorting remaining characters.
        /// </summary>
        /// <param name="input1">The first string to compare.</param>
        /// <param name="input2">The second string to compare.</param>
        /// <returns>True when the normalized character sets match; otherwise false.</returns>
        public static bool AlphaNumericMatch(string input1, string input2)
        {
            // Remove all non-alphanumeric characters
            var cleaned1 = Regex.Replace(input1, "[^a-zA-Z0-9]", "");
            var cleaned2 = Regex.Replace(input2, "[^a-zA-Z0-9]", "");

            // Sort characters in ascending order
            var sorted1 = new string(cleaned1.OrderBy(c => c).ToArray());
            var sorted2 = new string(cleaned2.OrderBy(c => c).ToArray());

            return sorted1 == sorted2;
        }

        /// <summary>
        /// Determines whether all readable properties on an object are null.
        /// </summary>
        /// <param name="obj">The object to inspect.</param>
        /// <returns>True when the object is null or every property value is null; otherwise false.</returns>
        public static bool AreObjectPropertiesNull(object obj)
        {
            if (obj == null)
            {
                return true; // If the object itself is null, all properties are considered null
            }

            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.GetValue(obj) == null)
                {
                    return false; // Found a property that is null
                }
            }

            return true; // All properties are null
        }

        /// <summary>
        /// Converts a list of strings into a single CSV line.
        /// </summary>
        /// <param name="data">The list of values to join.</param>
        /// <returns>A comma-separated string ending with a newline, or an empty string when no data is provided.</returns>
        public static string ListToCsvString(List<string> data)
        {
            if (data == null || data.Count == 0)
            {
                return string.Empty;
            }
            var csvBuilder = new StringBuilder();
            var rowStrings = data.ToList();
            csvBuilder.AppendLine(string.Join(",", rowStrings));
            return csvBuilder.ToString();
        }

        /// <summary>
        /// Determines whether a string is null, empty, or whitespace.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True when the value is null, empty, or whitespace; otherwise false.</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Truncates a string to the specified maximum length.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <returns>The original string when within the limit, otherwise a substring of the requested length.</returns>
        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Removes characters that are not letters, digits, underscores, periods, or hyphens.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The filtered string, or the original value when null or empty.</returns>
        public static string RemoveSpecialCharacters(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(value, "[^a-zA-Z0-9_.-]+", "");
        }

        /// <summary>
        /// Converts a string to title case using the current culture.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The title-cased string, or the original value when null or empty.</returns>
        public static string ToTitleCase(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        /// <summary>
        /// Encodes a UTF-8 string as Base64.
        /// </summary>
        /// <param name="value">The plain text to encode.</param>
        /// <returns>The Base64-encoded string, or the original value when null or empty.</returns>
        public static string Base64Encode(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }

        /// <summary>
        /// Decodes a Base64 string to UTF-8 text.
        /// </summary>
        /// <param name="value">The Base64-encoded string.</param>
        /// <returns>The decoded plain text, or the original value when null or empty.</returns>
        public static string Base64Decode(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var valueBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }

        /// <summary>
        /// Reverses the character order of a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The reversed string, or the original value when null or empty.</returns>
        public static string Reverse(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var valueChars = value.ToCharArray();
            Array.Reverse(valueChars);
            return new string(valueChars);
        }

        /// <summary>
        /// Removes all whitespace characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The string without whitespace, or the original value when null or empty.</returns>
        public static string RemoveWhitespace(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Removes all digit characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The string without digits, or the original value when null or empty.</returns>
        public static string RemoveDigits(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(c => !char.IsDigit(c)).ToArray());
        }

        /// <summary>
        /// Keeps only digit characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The digits-only string, or the original value when null or empty.</returns>
        public static string RemoveNonDigits(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Keeps only letter and digit characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The alphanumeric string, or the original value when null or empty.</returns>
        public static string RemoveNonAlphanumeric(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        /// Keeps only letter characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The letters-only string, or the original value when null or empty.</returns>
        public static string RemoveNonLetters(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsLetter).ToArray());
        }

        /// <summary>
        /// Keeps only uppercase letter characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The uppercase-letters-only string, or the original value when null or empty.</returns>
        public static string RemoveNonUppercaseLetters(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsUpper).ToArray());
        }

        /// <summary>
        /// Keeps only lowercase letter characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The lowercase-letters-only string, or the original value when null or empty.</returns>
        public static string RemoveNonLowercaseLetters(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsLower).ToArray());
        }

        /// <summary>
        /// Keeps only punctuation characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The punctuation-only string, or the original value when null or empty.</returns>
        public static string RemoveNonPunctuation(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsPunctuation).ToArray());
        }

        /// <summary>
        /// Keeps only symbol characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The symbols-only string, or the original value when null or empty.</returns>
        public static string RemoveNonSymbols(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsSymbol).ToArray());
        }

        /// <summary>
        /// Keeps only separator characters from a string.
        /// </summary>
        /// <param name="value">The input string.</param>
        /// <returns>The separators-only string, or the original value when null or empty.</returns>
        public static string RemoveNonSeparator(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return new string(value.ToCharArray().Where(char.IsSeparator).ToArray());
        }
    }
}