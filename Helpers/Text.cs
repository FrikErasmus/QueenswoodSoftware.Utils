namespace QueenswoodSoftware.Utils.Helpers
{
    public class Text
    {
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
    }
}